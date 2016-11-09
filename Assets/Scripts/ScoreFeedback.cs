using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreFeedback : MonoBehaviour {

	public Text bestScore;
	public Text lastScore;

	// Use this for initialization
	void Start () {
		lastScore.text += PlayerPrefsManager.GetLastScore();
		bestScore.text += PlayerPrefsManager.GetHighestScore();
	}
}
