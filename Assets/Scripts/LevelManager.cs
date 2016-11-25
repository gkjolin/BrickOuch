using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

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
			this.UpdateScoreOnPause();
			pausePanel.SetActive(true);
		} else {
			pausePanel.SetActive(false);
			Time.timeScale = 1f;
			paddle.freezePaddle = false;
		}
	}

	public void TogglePause ()
	{
		GameIsPaused = !GameIsPaused;
	}

	private void UpdateScoreOnPause() {
		GameObject scoreObj = GameObject.Find ("Score");
 		Score score = scoreObj.GetComponent<Score> ();
 		score.UpdateHighestScore();
	}

	private void TrackAndroidBackButton ()
	{
		if (Input.GetKey (KeyCode.Escape)) {
			GoogleAnalytics.HitAnalyticsEvent ("disabledbuttons", "android_back");
		}
	}

	public void ComingSoonEvents(string eventName)
	{
		GoogleAnalytics.HitAnalyticsEvent("coming_soon", eventName);
	}
}
