using UnityEngine;
using System.Collections;
using Facebook.Unity;
using Spine.Unity;
using SA.Analytics.Google;

public class LevelManager : MonoBehaviour
{
	private const float DigitSize = 120;

	public bool GameIsPaused { get; set; }

	public GameObject pausePanel;

	public int Phase { get; set; }

	private Paddle paddle;
	private MusicPlayer musicPlayer;

	public GameObject levelUpBackground;
	public GameObject levelUpNumber;

	private void Start ()
	{
		GameIsPaused = false;
		paddle = GameObject.FindObjectOfType<Paddle> ();
		musicPlayer = GameObject.FindObjectOfType<MusicPlayer> ();
		Phase = 1;
	}

	// Awake function from Unity's MonoBehavior
	void Awake ()
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
		GameObject scoreObj = GameObject.Find ("Score");
		Score score = scoreObj.GetComponent<Score> ();
		score.UpdateHighestScore ();
	}

	public void ComingSoonEvents (string eventName)
	{
		Manager.Client.SendEventHit ("coming_soon", eventName);
	}

	public void LevelUpAnimation(int level) {
		var levelStr = level.ToString ();
		var strSize = (levelStr.Length - 1) * DigitSize;
		var offset = -strSize / 2;

		foreach (var digit in levelStr) {
			CreateAnimation (levelUpNumber, digit.ToString (), offset, true);
			offset += DigitSize;
		}

		CreateAnimation (levelUpBackground);
	}

	private void CreateAnimation (GameObject prefab, string skin = null, float offset = 0, bool front = false)
	{
		var gameObj = Instantiate (prefab) as GameObject;
		gameObj.transform.Translate (offset, 0, front ? -5 : -4);

		var animation = gameObj.GetComponent<SkeletonAnimation> ();

		if (skin != null) {
			animation.skeleton.SetSkin (skin);
		}

		animation.state.AddAnimation (0, "Out", false, 2);
		DestroyOnComplete (animation);
	}

	private void DestroyOnComplete (SkeletonAnimation skeletonAnim)
	{
		skeletonAnim.state.End += delegate (Spine.AnimationState state, int trackIndex) {
			var animationName = state.GetCurrent(trackIndex).Animation.Name;

			if (animationName == "Out") {
				Destroy (skeletonAnim.gameObject);
			}
		};
	}

}
