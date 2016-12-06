using UnityEngine;
using System.Collections.Generic;
using Facebook.Unity;
using System;

public static class FacebookAccess
{
	private const string FB_ID = "facebook_id";
	private const string FB_NAME = "facebook_name";

	public static void SetName (string name)
	{
		PlayerPrefs.SetString (FB_NAME, name);
	}

	public static string GetName ()
	{
		return PlayerPrefs.GetString (FB_NAME);
	}

	public static void SetId (string id)
	{
		PlayerPrefs.SetString (FB_ID, id);
	}

	public static string GetId ()
	{
		return PlayerPrefs.GetString (FB_ID);
	}

	public static void Invite ()
	{
		FB.Mobile.AppInvite (new Uri ("https://fb.me/1267041886676012"), callback: delegate (IAppInviteResult result) {
			Debug.Log (result);
		});
	}

	public static void GetScores ()
    {
        FB.API("/app/scores?fields=score,user.limit(20)", HttpMethod.GET, GetScoresCallback);
    }

    private static void GetScoresCallback(IGraphResult result) 
    {
        Debug.Log("GetScoresCallback");
        if (result.Error != null)
        {
            Debug.LogError(result.Error);
            return;
        }
        Debug.Log(result.RawResult);

        // Parse scores info
        var scoresList = new List<object>();

        object scoresh;
        if (result.ResultDictionary.TryGetValue ("data", out scoresh)) 
        {
            scoresList = (List<object>) scoresh;
        }

        // Parse score data
        HandleScoresData (scoresList);
    }

	private static void HandleScoresData (List<object> scoresResponse)
    {
        foreach(object scoreItem in scoresResponse) 
		{
			var entry = (Dictionary <string,object>) scoreItem;
            var user = (Dictionary<string,object>) entry["user"];
            string userId = (string)user["id"];
            
            if (string.Equals(userId, AccessToken.CurrentAccessToken.UserId))
            {
                // This entry is the current player
                int playerHighScore = GetScoreFromEntry(entry);
                Debug.Log("Local players score on server is " + playerHighScore);
				var localHighScore = PlayerPrefsManager.GetHighestScore();
				if (playerHighScore < localHighScore)
                {
					Debug.Log("Locally overriding with just acquired score: " + localHighScore);
					playerHighScore = localHighScore;
                }
                
                entry["score"] = playerHighScore.ToString();
				PlayerPrefsManager.SetHighestScore(playerHighScore);
            }

			FBScore score = JsonMapping.GetScore (scoreItem);
			GameManager.Instance.Scores.Add (score);
        }
    }

	// Pull out score from a JSON user entry object constructed in FBGraph.GetScores()
    public static int GetScoreFromEntry(object obj)
    {
        Dictionary<string,object> entry = (Dictionary<string,object>) obj;
        return Convert.ToInt32(entry["score"]);
    }

	public static void PostScore (int score, Action callback = null)
    {
        // Check for 'publish_actions' as the Scores API requires it for submitting scores
        if (HavePublishActions)
        {
            var query = new Dictionary<string, string>();
            query["score"] = score.ToString();
            FB.API(
                "/me/scores",
                HttpMethod.POST,
                delegate(IGraphResult result)
            {
                Debug.Log("PostScore Result: " + result.RawResult);
                // Fetch fresh scores to update UI
                GetScores();
            },
            query
            );
        }
        else
        {
            // Showing context before prompting for publish actions
            // See Facebook Login Best Practices: https://developers.facebook.com/docs/facebook-login/best-practices
            /*PopupScript.SetPopup("Prompting for Publish Permissions for Scores API", 4f, delegate
            {*/
                // Prompt for `publish actions` and if granted, post score
                PromptForPublish(delegate
                                         {
                    if (HavePublishActions)
                    {
                        PostScore(score);
                    }
                });
            /*});*/
			Debug.Log("No publish rights");
        }
    }

	public static void PromptForPublish (Action callback = null)
    {
		List<string> publishPermissions = new List<string> {"publish_actions"};
        // Login for publish permissions
        // https://developers.facebook.com/docs/unity/reference/current/FB.LogInWithPublishPermissions
        FB.LogInWithPublishPermissions(publishPermissions, delegate (ILoginResult result)
        {
            Debug.Log("LoginCallback");
            if (FB.IsLoggedIn)
            {
                Debug.Log("Logged in with ID: " + AccessToken.CurrentAccessToken.UserId +
                          "\nGranted Permissions: " + AccessToken.CurrentAccessToken.Permissions.ToCommaSeparateList());
            }
            else
            {
                if (result.Error != null)
                {
                    Debug.LogError(result.Error);
                }
                Debug.Log("Not Logged In");
            }
            if (callback != null)
            {
                callback();
            }
        });
    }

    #region Util
    // Helper function to check whether the player has granted 'publish_actions'
    public static bool HavePublishActions
    {
        get {
            return (FB.IsLoggedIn &&
                   (AccessToken.CurrentAccessToken.Permissions as List<string>).Contains("publish_actions")) ? true : false;
        }
        private set {}
    }
    #endregion
}
