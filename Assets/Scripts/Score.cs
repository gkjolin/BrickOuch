﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Facebook.Unity;

public class Score : MonoBehaviour {

	private int score = 0;
	private int highestScore = 0;
	private Text scoreText;

	// Use this for initialization
	void Start () {
		highestScore = PlayerPrefsManager.GetHighestScore();
		scoreText = GetComponent<Text> ();
		scoreText.text = string.Format ("{0}", score);
	}

	public void AddScore(int points = 10) {
		score += points;
		scoreText.text = string.Format ("{0}", score);
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
		PlayerPrefsManager.SetLastScore(score);

		if (score > highestScore)
		{
			highestScore = score;
			PlayerPrefsManager.SetHighestScore(score);
			if(FB.IsLoggedIn){
				FacebookAccess.PostScore(score);
				}
		}
	}

	void OnDisable()
	{
		UpdateHighestScore();
    }
}
