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

	public void FacebookInvite()
	{
		FacebookAccess.Invite();
	}

	public void  Login ()
	{
		var perms = new List<string> () { "public_profile", "user_friends", "publish_actions" };
		FB.LogInWithPublishPermissions (perms, AuthCallback);
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
			FacebookAccess.GetScores ();
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
		}
	}
}
