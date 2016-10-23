using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreFeedback : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject scoreObj = GameObject.Find ("Score");
		Score score = scoreObj.GetComponent<Score> ();

		Text text = this.GetComponent<Text> ();
		text.text = "Score\n" + score.GetScore ();

		Destroy (scoreObj.transform.root.gameObject);
	}
}
