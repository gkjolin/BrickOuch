using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using SA.Analytics.Google;

public class GameManager : Singleton<GameManager> {

	public HashSet<FBScore> Scores { get; private set; }

	protected override void Initialize ()
	{
		Scores = new HashSet<FBScore> ();
	}

	void Update ()
	{
		TrackAndroidBackButton ();
	}

	void OnApplicationFocus (bool hasFocus) {
		if (hasFocus) {
			FacebookAccess.GetScores ();
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
