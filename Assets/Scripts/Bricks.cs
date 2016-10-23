using UnityEngine;
using System.Collections;

public class Bricks : MonoBehaviour {

	string path = "Prefabs/green";
	GameObject[,] bricks = new GameObject[6, 16];

	// Use this for initialization
	void Start () {
		for (int i = 0; i < bricks.GetLength(0); i++) {
			for (int j = 0; j < bricks.GetLength(1); j++) {
				bricks [i, j] = Instantiate (Resources.Load (path)) as GameObject;

				Vector2 startPos = new Vector2 (150 * i, 50 * j + 800);
				bricks [i, j].transform.position = startPos;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
