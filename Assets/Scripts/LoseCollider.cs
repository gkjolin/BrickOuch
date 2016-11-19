using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoseCollider : MonoBehaviour
{
	public int lives = 3;

	public LevelManager levelManager;
	public Paddle paddle;
	public Ball ball;
	public GameObject lifeIcon;
	public GameObject lifeContainer;

	void Start()
	{
		DrawLives ();
	}

	void OnTriggerEnter2D (Collider2D trigger)
	{
		if (trigger.CompareTag("Ball"))
		{
			ball.PuffAnimation ();

			if (lives > 0)
			{
				UseExtraLife ();
				StartCoroutine (ResetGame());
			}
			else
			{
				paddle.EndGameAnimation ();
				StartCoroutine (LoadLoseScreen ());
			}
		}
	}

	private void UseExtraLife ()
	{
		lives--;

		var lifeIcon = lifeContainer.transform.GetChild(lives).gameObject;
		Destroy(lifeIcon);
	}

	private void DrawLives()
	{
		lives--;

		for (int i = 0; i < lives; i++) 
		{
			var life = Instantiate(lifeIcon, lifeContainer.transform, false) as GameObject;
			float offset = i * life.GetComponent<RectTransform>().rect.width;
			Vector2 position = new Vector2(offset, 0);

			life.transform.localPosition = position;
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
