using UnityEngine;
using System.Collections;

public class Bricks : MonoBehaviour {

	private const int MaxBricks = 50;
	string path = "Prefabs/yellow";
	GameObject[,] bricks = new GameObject[6, 16];

	// Use this for initialization
	void Start () {
		for (int i = 0; i < MaxBricks; i++) {
			CreateRandomBrick ();
		}
	}
	
	public void CreateRandomBrick() {
		if (Brick.breakableCount < MaxBricks) {
			int posX = Random.Range (0, 6);
			int posY = Random.Range (0, 16);
			CreateBrick (posX, posY);
		}
	}

	private void CreateBrick(int posX, int posY) {
		if (bricks [posX, posY] == null) {
			bricks [posX, posY] = Instantiate (Resources.Load (path)) as GameObject;
			
			Vector2 startPos = new Vector2 (150 * posX, 50 * posY + 800);
			bricks [posX, posY].transform.position = startPos;
		}
	}
}
