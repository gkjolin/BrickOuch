using UnityEngine;
using System.Collections;

public class LoseCollider : MonoBehaviour
{
	public int lives = 3;

	public LevelManager levelManager;
	public Paddle paddle;
	public Ball ball;

	void OnTriggerEnter2D (Collider2D trigger)
	{
		if (trigger.CompareTag("Ball"))
		{
			lives--;
			ball.PuffAnimation ();

			if (lives == 0)
			{
				paddle.EndGameAnimation ();
				StartCoroutine (LoadLoseScreen ());
			}
			else
			{
				StartCoroutine (ResetGame());
			}
		}
	}

	private IEnumerator ResetGame()
	{
		yield return new WaitForSeconds (2f);
		ball.Reset(levelManager.Phase);
		paddle.Reset();
	}

	private IEnumerator LoadLoseScreen ()
	{
		yield return new WaitForSeconds (2f);
		levelManager.LoadScene ("Start Menu");
	}
	
}
