using UnityEngine;
using System.Collections.Generic;
using Facebook.Unity;
using System;

public class FacebookAccess : MonoBehaviour
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
		FB.Mobile.AppInvite (
			new Uri ("https://fb.me/810530068992919"));
	}
}
