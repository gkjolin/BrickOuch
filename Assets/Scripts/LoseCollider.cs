using UnityEngine;
using System.Collections;

public class LoseCollider : MonoBehaviour
{
	private LevelManager levelManager;
	private Paddle paddle;

	void Start ()
	{
		levelManager = GameObject.FindObjectOfType<LevelManager> ();
		paddle = GameObject.FindObjectOfType<Paddle> ();
	}

	void OnTriggerEnter2D (Collider2D trigger)
	{
		paddle.EndGameAnimation ();
		StartCoroutine (LoadLoseScreen ());
	}

	private IEnumerator LoadLoseScreen ()
	{
		yield return new WaitForSeconds (2f);
		levelManager.LoadScene ("Score Screen");
	}
	
}
