using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Facebook.Unity;
using System;

public class FacebookPanelController : MonoBehaviour
{

	public Text welcomeText;
	public Button loginButton;

	void Awake ()
	{
		if (FB.IsLoggedIn) {
			loginButton.GetComponentInChildren<Text> ().text = "Logout";
			welcomeText.text = "Welcome, " + FacebookAccess.GetName ();
		} else {
			loginButton.GetComponentInChildren<Text> ().text = "Login";
			welcomeText.text = "Welcome";
		}
	}

	public void  Login ()
	{
		var perms = new List<string> () { "public_profile", "user_friends" };
		FB.LogInWithReadPermissions (perms, AuthCallback);
	}

	private void AuthCallback (ILoginResult result)
	{
		if (FB.IsLoggedIn) {
			// AccessToken class will have session details
			var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
			// Print current access token's User ID
			Debug.Log (aToken.UserId);
			// Print current access token's granted permissions
			foreach (string perm in aToken.Permissions) {
				Debug.Log (perm);
			}
			FB.API ("/me", HttpMethod.GET, FBAPICallback);
		} else {
			Debug.Log ("User cancelled login");
		}
	}

	private void FBAPICallback (IResult result)
	{
		if (!String.IsNullOrEmpty (result.Error)) {
			// Handle error
		} else {
			// Got user profile info
			var resultObject = result.ResultDictionary;
			var facebookId = resultObject["id"];
			var facebookName = resultObject["name"];

			loginButton.GetComponentInChildren<Text> ().text = "Logout";
			welcomeText.text = "Welcome, " + facebookName;

			FacebookAccess.SetId((string)facebookId);
			FacebookAccess.SetName((string)facebookName);

			/*var userProfile = new Dictionary<string, string> ();
             
			userProfile ["facebookId"] = getDataValueForKey (resultObject, "id");
			userProfile ["name"] = getDataValueForKey (resultObject, "name");
			object location;
			if (resultObject.TryGetValue ("location", out location)) {
				userProfile ["location"] = (string)(((Dictionary<string, object>)location) ["name"]);
			}
			userProfile ["gender"] = getDataValueForKey (resultObject, "gender");
			userProfile ["birthday"] = getDataValueForKey (resultObject, "birthday");
			userProfile ["relationship"] = getDataValueForKey (resultObject, "relationship_status");
			if (userProfile ["facebookId"] != "") {
				userProfile ["pictureURL"] = 
                "https://graph.facebook.com/" + userProfile ["facebookId"] + "/picture?type=large&return_ssl_resources=1";
			}
             
			var emptyValueKeys = userProfile
            .Where (pair => String.IsNullOrEmpty (pair.Value))
            .Select (pair => pair.Key).ToList ();
			foreach (var key in emptyValueKeys) {
				userProfile.Remove (key);
			}
             
			StartCoroutine ("saveUserProfile", userProfile);*/
		}
	}
}
