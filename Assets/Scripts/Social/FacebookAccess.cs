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
	public Sprite Picture { get; set; }

	protected override bool Destroyable {
		get {
			return false;
		}
	}

	protected override void Initialize ()
	{
		if (!FB.IsInitialized) {
			// Initialize the Facebook SDK
			FB.Init (InitCallback, OnHideUnity);
		} else {
			// Already initialized, signal an app activation App Event
			FB.ActivateApp ();
		}
	}

	private void InitCallback ()
	{
		if (FB.IsInitialized) {
			// Signal an app activation App Event
			FB.ActivateApp ();
			// Continue with Facebook SDK
			// ...
		} else {
			Debug.Log ("Failed to Initialize the Facebook SDK");
		}
	}

	private void OnHideUnity (bool isGameShown)
	{
		if (!isGameShown) {
			// Pause the game - we will need to hide
			Time.timeScale = 0;
		} else {
			// Resume the game - we're getting focus again
			Time.timeScale = 1;
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

			LoadPicture ();
			PlayfabAccess.Instance.Login (aToken.TokenString);
		} else {
			Debug.Log ("User cancelled login");
		}
	}

	private void LoadPicture () {
		FB.API ("/me/picture", HttpMethod.GET, PictureCallback);
	}

	private void PictureCallback (IGraphResult result)
	{
		if (String.IsNullOrEmpty (result.Error) && !result.Cancelled && result.Texture != null) {
			var rect = new Rect (0, 0, result.Texture.width, result.Texture.height);
			Picture = Sprite.Create (result.Texture, rect, Vector2.zero);
		} else {
			Picture = null;
			Debug.Log ("Failed loading profile picture");
		}
	}

}
