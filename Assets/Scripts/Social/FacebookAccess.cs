using UnityEngine;
using System.Collections.Generic;
using Facebook.Unity;
using System;

public class FacebookAccess : Singleton<FacebookAccess>
{
	public string Id { get; set; }
	public string Name { get; set; }

	protected override bool Destroyable {
		get {
			return false;
		}
	}

	public void Invite ()
	{
		FB.Mobile.AppInvite (new Uri ("https://fb.me/1267041886676012"), callback: delegate (IAppInviteResult result) {
			Debug.Log (result);
		});
	}

	public void Login ()
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

			PlayfabAccess.Instance.Login (aToken.TokenString);
		} else {
			Debug.Log ("User cancelled login");
		}
	}

}
