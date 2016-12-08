using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoseCollider : MonoBehaviour
{

	public LevelManager levelManager;
	public Paddle paddle;
	public Ball ball;

	void OnTriggerEnter2D (Collider2D trigger)
	{
		if (trigger.CompareTag("Ball"))
		{
			ball.PuffAnimation ();

			if (paddle.Lives > 0)
			{
				StartCoroutine (ResetGame());
			}
			else
			{
				paddle.EndGameAnimation ();
				StartCoroutine (LoadLoseScreen ());
			}
		}
	}

	private IEnumerator ResetGame()
	{
		yield return new WaitForSeconds (1f);
		ball.Reset ();
		ball.ReadyToLaunch (levelManager.Phase);
		paddle.Reset();
	}

	private IEnumerator LoadLoseScreen ()
	{
		yield return new WaitForSeconds (2f);
		GameManager.Instance.LoadScene ("Start Menu");
	}
	
}
