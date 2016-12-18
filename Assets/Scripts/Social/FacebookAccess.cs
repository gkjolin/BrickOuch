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
	public Dictionary<string, Sprite> FriendsPictures { get; private set; }

	protected override bool Destroyable {
		get {
			return false;
		}
	}

	protected override void Initialize ()
	{
		FriendsPictures = new Dictionary<string, Sprite> ();

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
			LoadFriendsPictures ();
			PlayfabAccess.Instance.Login (aToken.TokenString);
		} else {
			EventManager.Instance.FacebookLoginCancel ();
		}
	}

	private void LoadPicture () {
		FB.API ("/me/picture", HttpMethod.GET, PictureCallback);
	}

	private void PictureCallback (IGraphResult result)
	{
		if (String.IsNullOrEmpty (result.Error) && !result.Cancelled && result.Texture != null) {
			Picture = ConvertTextureToSprite (result.Texture);
		} else {
			Picture = null;
			Debug.Log ("Failed loading profile picture");
		}
	}

	public void LoadFriendsPictures () {
		FB.API ("/me/friends?fields=id,name,picture", HttpMethod.GET, FriendsPicturesCallback);
	}

	private void FriendsPicturesCallback (IGraphResult result)
	{
		if (String.IsNullOrEmpty (result.Error) && !result.Cancelled) {
			RequestFriendsPictures (result.ResultDictionary);
		} else {
			Debug.Log ("Failed loading friends pictures");
		}
	}

	private void RequestFriendsPictures (IDictionary<string, object> friends)
	{
		var dataList = (List<object>)friends ["data"];

		foreach (var data in dataList) {
			var friend = (Dictionary<string, object>)data;
			var id = (string)friend ["id"];

			var picture = (Dictionary<string, object>)friend ["picture"];
			var pictureData = (Dictionary<string, object>)picture ["data"];
			var url = (string)pictureData ["url"];

			StartCoroutine (GetImage(id, url));
		}
	}

	private IEnumerator GetImage (string id, string url)
	{
		WWW www = new WWW(url);
		yield return www;

		if (www.error != null)
		{
			Debug.LogError(www.error);
			yield break;
		}

		var picture = ConvertTextureToSprite (www.texture);
		FriendsPictures[id] = picture;
	}

	private Sprite ConvertTextureToSprite (Texture2D texture)
	{
		var rect = new Rect (0, 0, texture.width, texture.height);
		return Sprite.Create (texture, rect, Vector2.zero);
	}
}
