﻿using UnityEngine;
using System;
using System.Collections;
using Facebook.Unity;
using Spine.Unity;
using SA.Analytics.Google;

public class LevelManager : Singleton<LevelManager>
{
	private const float DigitSize = 120;

	public bool GameIsPaused { get; set; }

	public GameObject pausePanel;

	public int Phase { get; set; }

	private Paddle paddle;
	private MusicPlayer musicPlayer;

	public GameObject levelUpOverlay;
	public GameObject levelUpBackground;
	public GameObject levelUpNumber;

	protected override bool Destroyable {
		get {
			return true;
		}
	}

	private void Start ()
	{
		GameIsPaused = false;
		paddle = GameObject.FindObjectOfType<Paddle> ();
		musicPlayer = GameObject.FindObjectOfType<MusicPlayer> ();
		Phase = 1;
	}

	public void QuitRequest ()
	{
		Application.Quit ();
	}

	public void PauseGame ()
	{
		TogglePause ();

		if (musicPlayer) 
		{
			musicPlayer.ToggleMute ();
		}

		if (GameIsPaused) {
			pausePanel.SetActive (true);
			Time.timeScale = 0f;
			paddle.freezePaddle = true;
			this.UpdateScoreOnPause ();
		} else {
			pausePanel.SetActive (false);
			Time.timeScale = 1f;
			paddle.freezePaddle = false;
		}
	}

	public void TogglePause ()
	{
		GameIsPaused = !GameIsPaused;
	}

	private void UpdateScoreOnPause ()
	{
		Score score = GameObject.FindObjectOfType<Score> ();
		score.UpdateHighestScore ();
	}

	public void ComingSoonEvents (string eventName)
	{
		Manager.Client.SendEventHit ("coming_soon", eventName);
	}

	public void LevelUpAnimation(int level, Action callback) {
		var levelStr = level.ToString ();
		var strSize = (levelStr.Length - 1) * DigitSize;
		var offset = -strSize / 2 + 5;

		levelUpOverlay.SetActive (true);
		foreach (var digit in levelStr) {
			CreateAnimation (levelUpNumber, digit.ToString (), offset, true);
			offset += DigitSize;
		}

		CreateAnimation (levelUpBackground, callback: callback);
	}

	private void CreateAnimation (GameObject prefab, string skin = null, float offset = 0, bool front = false, Action callback = null)
	{
		var gameObj = Instantiate (prefab) as GameObject;
		gameObj.transform.Translate (offset, 0, front ? -5 : -4);

		var animation = gameObj.GetComponent<SkeletonAnimation> ();

		if (skin != null) {
			animation.skeleton.SetSkin (skin);
		}

		animation.state.AddAnimation (0, "Out", false, 2);
		DestroyOnComplete (animation, callback);
	}

	private void DestroyOnComplete (SkeletonAnimation skeletonAnim, Action callback)
	{
		skeletonAnim.state.End += delegate (Spine.AnimationState state, int trackIndex) {
			var animationName = state.GetCurrent(trackIndex).Animation.Name;

			if (animationName == "Out") {
				Destroy (skeletonAnim.gameObject);

				if (callback != null) {
					levelUpOverlay.SetActive (false);
					callback();
				}
			}
		};
	}

}
