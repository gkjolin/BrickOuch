using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public static GameManager Instance { get; private set; }

	public HashSet<FBScore> Scores { get; private set; }

	void Awake ()
	{
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad (this);

			// Initialize GameManager variables
			Scores = new HashSet<FBScore> ();
		} else {
			Destroy (gameObject);
		}
	}
}
