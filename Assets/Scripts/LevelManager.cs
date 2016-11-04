using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour
{
	public bool GameIsPaused { get; set; }

	private Paddle paddle;

	private void Start()
	{
		GameIsPaused = false;
		paddle = GameObject.FindObjectOfType<Paddle>();
	}

	public void LoadScene (string name)
	{
		Brick.breakableCount = 0;
		SceneManager.LoadScene (name);
	}

	public void QuitRequest ()
	{
		Application.Quit ();
	}

	public void PauseGame()
	{
		TogglePause();

		if (GameIsPaused) 
		{
			Time.timeScale = 0f;
			paddle.freezePaddle = true;
		} 
		else 
		{
			Time.timeScale = 1f;
			paddle.freezePaddle = false;
		}
	}

	public void TogglePause()
	{
		GameIsPaused = !GameIsPaused;
	}
}
