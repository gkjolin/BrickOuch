using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Score : MonoBehaviour {

	int score = 0;
	Text scoreText;

	// Use this for initialization
	void Start () {
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

}
