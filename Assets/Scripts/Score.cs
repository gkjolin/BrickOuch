using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Score : MonoBehaviour {

	private int score = 0;
	private int highestScore = 0;
	private Text scoreText;
	private const string HIGHEST_SCORE_KEY = "HIGHEST SCORE";

	// Use this for initialization
	void Start () {
		highestScore = PlayerPrefs.GetInt(HIGHEST_SCORE_KEY, 0);
		scoreText = GetComponent<Text> ();
		DontDestroyOnLoad (gameObject.transform.root.gameObject);
	}

	public void AddScore(int points = 10) {
		score += points;
		scoreText.text = string.Format ("Score: {0}", score);
	}

	public int GetScore() {
		return score;
	}

	public int GetHighestScore()
	{
		UpdateHighestScore();

		return highestScore;
	}

	public void UpdateHighestScore()
	{
		if (score > highestScore)
		{
			highestScore = score;
			PlayerPrefs.SetInt(HIGHEST_SCORE_KEY, score);
			PlayerPrefs.Save();
		}
	}

	void OnDisable()
	{
		UpdateHighestScore();
    }
}
