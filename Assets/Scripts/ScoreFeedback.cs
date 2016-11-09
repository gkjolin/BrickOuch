using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreFeedback : MonoBehaviour {

	public Text bestScore;
	public Text lastScore;

	// Use this for initialization
	void Start () {
		GameObject scoreObj = GameObject.Find ("Score");
		Score score = scoreObj.GetComponent<Score> ();

		lastScore.text += score.GetScore ();
		bestScore.text += score.GetHighestScore();

		Destroy (scoreObj.transform.root.gameObject);
	}
}
