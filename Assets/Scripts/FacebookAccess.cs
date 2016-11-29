using UnityEngine;
using System.Collections.Generic;
using Facebook.Unity;
using System;

public class FacebookAccess : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static void Login()
	{
		var perms = new List<string>() { "public_profile", "user_friends" };
		FB.LogInWithReadPermissions(perms, FacebookAccess.AuthCallback);
	}

	private static void AuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            // AccessToken class will have session details
            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
            // Print current access token's User ID
            Debug.Log(aToken.UserId);
            // Print current access token's granted permissions
            foreach (string perm in aToken.Permissions)
            {
                Debug.Log(perm);
            }
        }
        else
        {
            Debug.Log("User cancelled login");
        }
    }

    public static void Invite()
    {
		FB.Mobile.AppInvite(
    new Uri("https://fb.me/810530068992919"));
    }
}
