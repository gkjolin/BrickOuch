using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Score : MonoBehaviour {

	int score = 0;
	Text scoreText;

	// Use this for initialization
	void Start () {
		scoreText = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void AddScore() {
		score += 10;
		scoreText.text = string.Format ("Score: {0}", score);
	}
}
