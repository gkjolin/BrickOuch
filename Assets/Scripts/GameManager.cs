using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public static GameManager Instance { get; private set; }

	static GameManager ()
	{
		GameObject container = new GameObject("GameManager");
		Instance = container.AddComponent<GameManager> ();
	}

	public HashSet<FBScore> Scores { get; private set; }

	private GameManager()
	{
		Scores = new HashSet<FBScore> ();
	}

	void Awake()
	{
		DontDestroyOnLoad (this);
	}
}
