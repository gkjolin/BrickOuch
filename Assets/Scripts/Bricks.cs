using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bricks : MonoBehaviour {

	private const int MaxBricks = 50;
	private const float brickCreationIndex = 1.5f;
	private GameObject[,] bricks = new GameObject[6, 16];

	public List<GameObject> prefabs;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < MaxBricks; i++) {
			CreateRandomBrick ();
		}
	}

	void Update ()
	{
		float createBrickProbability = brickCreationIndex/(Brick.breakableCount + 1f) * Time.deltaTime/Time.maximumDeltaTime;
		Debug.Log(createBrickProbability);
		if (Random.value < createBrickProbability)
		{
			CreateRandomBrick();
		}
	}
	
	public void CreateRandomBrick() {
		if (Brick.breakableCount < MaxBricks) {
			int type = UnityEngine.Random.Range (0, prefabs.Count);
			int posX = UnityEngine.Random.Range (0, 6);
			int posY = UnityEngine.Random.Range (0, 16);

			CreateBrick (type, posX, posY);
		}
	}

	private void CreateBrick(int type, int posX, int posY) {
		if (bricks [posX, posY] == null) {
			bricks [posX, posY] = Instantiate (prefabs[type]);
			bricks [posX, posY].transform.parent = transform;

			Vector2 startPos = new Vector2 (150 * posX + 75, 50 * posY + 700);
			bricks [posX, posY].transform.position = startPos;
		}
	}
}
