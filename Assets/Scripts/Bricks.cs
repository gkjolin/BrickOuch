using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bricks : MonoBehaviour {

	private const int MaxBricks = 50;
	private Dictionary<BrickType, string> brickPaths = new Dictionary<BrickType, string> () {
		{ BrickType.Yellow, "Prefabs/yellow" },
		{ BrickType.Green, "Prefabs/green" },
		{ BrickType.Red, "Prefabs/red" }
	};

	GameObject[,] bricks = new GameObject[6, 16];

	// Use this for initialization
	void Start () {
		for (int i = 0; i < MaxBricks; i++) {
			CreateRandomBrick ();
		}
	}
	
	public void CreateRandomBrick() {
		if (Brick.breakableCount < MaxBricks) {
			BrickType type = (BrickType)Random.Range (0, 3);
			int posX = Random.Range (0, 6);
			int posY = Random.Range (0, 16);

			CreateBrick (type, posX, posY);
		}
	}

	private void CreateBrick(BrickType type, int posX, int posY) {
		if (bricks [posX, posY] == null) {
			bricks [posX, posY] = Instantiate (Resources.Load (brickPaths[type])) as GameObject;
			
			Vector2 startPos = new Vector2 (150 * posX, 50 * posY + 800);
			bricks [posX, posY].transform.position = startPos;
		}
	}
}

public enum BrickType {
	Green, Yellow, Red
}
