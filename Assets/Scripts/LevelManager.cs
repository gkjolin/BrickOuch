using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Facebook.Unity;

public class LevelManager : MonoBehaviour
{
	public bool GameIsPaused { get; set; }

	public GameObject pausePanel;

	public int Phase { get; set; }

	private Paddle paddle;
	private MusicPlayer musicPlayer;

	private void Start ()
	{
		GameIsPaused = false;
		paddle = GameObject.FindObjectOfType<Paddle> ();
		musicPlayer = GameObject.FindObjectOfType<MusicPlayer> ();
		Phase = 1;
	}

	void Update ()
	{
		if (Application.platform == RuntimePlatform.Android) {
			this.TrackAndroidBackButton ();
		}
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

	public void FacebookLogin()
	{
		FacebookAccess.Login();
	}

	public void FacebookInvite()
	{
		FacebookAccess.Invite();
	}


	public void LoadScene (string name)
	{
		SceneManager.LoadScene (name);
	}

	public void QuitRequest ()
	{
		Application.Quit ();
	}

	public void PauseGame ()
	{
		TogglePause ();
		musicPlayer.ToggleMute ();

		if (GameIsPaused) {
			Time.timeScale = 0f;
			paddle.freezePaddle = true;
			this.UpdateScoreOnPause ();
			pausePanel.SetActive (true);
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

	private void TrackAndroidBackButton ()
	{
		if (Input.GetKey (KeyCode.Escape)) {
			GoogleAnalytics.HitAnalyticsEvent ("disabledbuttons", "android_back");
		}
	}

	public void ComingSoonEvents (string eventName)
	{
		GoogleAnalytics.HitAnalyticsEvent ("coming_soon", eventName);
	}
}
