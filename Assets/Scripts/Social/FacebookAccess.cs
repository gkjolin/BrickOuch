using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;

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

	public void Login (Image picture)
	{
		var perms = new List<string> () { "public_profile", "user_friends" };
		FB.LogInWithReadPermissions (perms, (result) => {
			AuthCallback (result, picture);
		});
	}

	private void AuthCallback (ILoginResult result, Image picture)
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

			LoadPicture (picture);
			PlayfabAccess.Instance.Login (aToken.TokenString);
		} else {
			Debug.Log ("User cancelled login");
		}
	}

	private void LoadPicture (Image picture) {
		FB.API ("/me/picture", HttpMethod.GET, (result) => {
			PictureCallback (result, picture);
		});
	}

	private void PictureCallback (IGraphResult result, Image picture)
	{
		if (String.IsNullOrEmpty (result.Error) && !result.Cancelled && result.Texture != null) {
			var rect = new Rect (0, 0, result.Texture.width, result.Texture.height);
			picture.sprite = Sprite.Create (result.Texture, rect, Vector2.zero);
			picture.gameObject.SetActive (true);
		} else {
			Debug.Log ("Failed loading profile picture");
		}
	}
}
