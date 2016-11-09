using UnityEngine;
using System.Collections;

public class LoseCollider : MonoBehaviour
{
	private LevelManager levelManager;
	private Paddle paddle;
	private Ball ball;

	void Start ()
	{
		levelManager = GameObject.FindObjectOfType<LevelManager> ();
		paddle = GameObject.FindObjectOfType<Paddle> ();
		ball = GameObject.FindObjectOfType<Ball> ();
	}

	void OnTriggerEnter2D (Collider2D trigger)
	{
		if (trigger.CompareTag("Ball"))
		{
			ball.PuffAnimation ();
			paddle.EndGameAnimation ();
			StartCoroutine (LoadLoseScreen ());
		}
	}

	private IEnumerator LoadLoseScreen ()
	{
		yield return new WaitForSeconds (2f);
		levelManager.LoadScene ("Start Menu");
	}
	
}
