using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Facebook.Unity;
using PlayFab;
using PlayFab.ClientModels;
using System;

public class FacebookPanelController : MonoBehaviour
{

	public Text welcomeText;
	public Button loginButton;
	public string PlayFabId;

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

	public void FacebookInvite ()
	{
		FacebookAccess.Invite ();
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
			this.PlayfabLogin (aToken);
			/*
			FB.API ("/me", HttpMethod.GET, FBAPICallback);
			FacebookAccess.GetScores ();*/
		} else {
			Debug.Log ("User cancelled login");
		}
	}

	private void PlayfabLogin (AccessToken facebookToken)
	{
		LoginWithFacebookRequest request = new LoginWithFacebookRequest () {
			TitleId = PlayFabSettings.TitleId,
			AccessToken = facebookToken.TokenString,
			CreateAccount = true,
		};

		PlayFabClientAPI.LoginWithFacebook (request, (result) => {
			PlayFabId = result.PlayFabId;
			Debug.Log ("Got PlayFabID: " + PlayFabId);

			if (result.NewlyCreated) {
				Debug.Log ("(new account)");
			} else {
				Debug.Log ("(existing account)");
			}

			this.GetUserData ();
		},
			(error) => {
				Debug.Log ("Error logging in player with custom ID:");
				Debug.Log (error.ErrorMessage);
			});
	}

	void GetUserData ()
	{
		GetAccountInfoRequest request = new GetAccountInfoRequest () {
			PlayFabId = PlayFabId
		};

		PlayFabClientAPI.GetAccountInfo (request, (result) => {
			Debug.Log ("Got account info:");
			if ((result.AccountInfo == null)) {
				Debug.Log ("No account available");
			} else {
				UserFacebookInfo fbInfo = result.AccountInfo.FacebookInfo;
				if (fbInfo == null) {
					Debug.Log ("No facebook info available");
				} else {
					loginButton.GetComponentInChildren<Text> ().text = "Logout";
					welcomeText.text = "Welcome, " + fbInfo.FullName;

					FacebookAccess.SetId ((string)fbInfo.FacebookId);
					FacebookAccess.SetName ((string)fbInfo.FullName);
				}

			}
		}, (error) => {
			Debug.Log ("Got error retrieving user data:");
			Debug.Log (error.ErrorMessage);
		});
	}

	private void FBAPICallback (IResult result)
	{
		if (!String.IsNullOrEmpty (result.Error)) {
			// Handle error
		} else {
			// Got user profile info
			var resultObject = result.ResultDictionary;
			var facebookId = resultObject ["id"];
			var facebookName = resultObject ["name"];

			loginButton.GetComponentInChildren<Text> ().text = "Logout";
			welcomeText.text = "Welcome, " + facebookName;

			FacebookAccess.SetId ((string)facebookId);
			FacebookAccess.SetName ((string)facebookName);
		}
	}
}
