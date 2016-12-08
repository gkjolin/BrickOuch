using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager> {

	public HashSet<FBScore> Scores { get; private set; }

	protected override void Initialize ()
	{
		Scores = new HashSet<FBScore> ();
	}

	void OnApplicationFocus (bool hasFocus) {
		if (hasFocus) {
			FacebookAccess.GetScores ();
		}
	}
}
