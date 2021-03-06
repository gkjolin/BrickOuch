﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using SA.Analytics.Google;

public class GameManager : Singleton<GameManager> {

	protected override bool Destroyable {
		get {
			return false;
		}
	}

	protected override void Initialize ()
	{
		// Initialize other singletons
		new GameObject ("FacebookAccess", typeof(FacebookAccess));
		new GameObject ("PlayfabAccess", typeof(PlayfabAccess));
		new GameObject ("EventManager", typeof(EventManager));
	}

	void Update ()
	{
		TrackAndroidBackButton ();
	}

	void OnApplicationFocus (bool hasFocus) {
		if (hasFocus) {
			PlayfabAccess.Instance.GetLeaderboard ();
		}
	}

	public void LoadScene (string name) {
		SceneManager.LoadScene (name);
	}

	private void TrackAndroidBackButton ()
	{
		if (Application.platform == RuntimePlatform.Android && Input.GetKey (KeyCode.Escape)) {
			Manager.Client.SendEventHit ("disabledbuttons", "android_back", SceneManager.GetActiveScene().name);
		}
	}
}
