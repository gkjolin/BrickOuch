using UnityEngine;
using System.Collections.Generic;
using Facebook.Unity;
using System;

public static class FacebookAccess
{
	private const string FB_ID = "facebook_id";
	private const string FB_NAME = "facebook_name";

	private static List<object> scores;
    public static List<object> Scores {
        get { return scores; }
        set { scores = value;}
    }

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
		FB.Mobile.AppInvite (
			new Uri ("https://fb.me/810530068992919"));
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
        var structuredScores = new List<object>();
        foreach(object scoreItem in scoresResponse) 
        {
            // Score JSON format
            // {
            //   "score": 4,
            //   "user": {
            //      "name": "Chris Lewis",
            //      "id": "10152646005463795"
            //   }
            // }

            var entry = (Dictionary<string,object>) scoreItem;
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
            
            structuredScores.Add(entry);
            /*if (!GameStateManager.FriendImages.ContainsKey(userId))
            {
                // We don't have this players image yet, request it now
                LoadFriendImgFromID (userId, pictureTexture =>
                {
                    if (pictureTexture != null)
                    {
                        GameStateManager.FriendImages.Add(userId, pictureTexture);
                        GameStateManager.CallUIRedraw();
                    }
                });
            }*/
        }

        Scores = structuredScores;
    }

	// Pull out score from a JSON user entry object constructed in FBGraph.GetScores()
    public static int GetScoreFromEntry(object obj)
    {
        Dictionary<string,object> entry = (Dictionary<string,object>) obj;
        return Convert.ToInt32(entry["score"]);
    }
}
