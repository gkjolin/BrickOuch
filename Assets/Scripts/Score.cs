using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Facebook.Unity;

public class Score : Singleton<Score>
{

	public int ScoreCount { get; private set; }
	private int highestScore = 0;
	private Text scoreText;

	protected override bool Destroyable {
		get {
			return true;
		}
	}

	protected override void Initialize ()
	{
		ScoreCount = 0;
	}

	// Use this for initialization
	void Start ()
	{
		highestScore = PlayerPrefsManager.GetHighestScore ();
		scoreText = GetComponent<Text> ();
		scoreText.text = string.Format ("{0}", ScoreCount);
	}

	public void AddScore (int points = 10)
	{
		ScoreCount += points;
		scoreText.text = string.Format ("{0}", ScoreCount);
	}

	public int GetScore ()
	{
		return ScoreCount;
	}

	public void UpdateHighestScore ()
	{
		PlayerPrefsManager.SetLastScore (ScoreCount);

		if (ScoreCount > highestScore) {
			highestScore = ScoreCount;
			PlayerPrefsManager.SetHighestScore (ScoreCount);
		}
	}

	void OnDisable ()
	{
		UpdateHighestScore ();
	}
}
