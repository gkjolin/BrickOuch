using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreFeedback : MonoBehaviour {

	public Text bestScore;
	public Text lastScore;

	void OnEnable()
    {
		lastScore.text = PlayerPrefsManager.GetLastScore().ToString();
		bestScore.text = PlayerPrefsManager.GetHighestScore().ToString();
    }
}
