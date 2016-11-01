using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bricks : MonoBehaviour {

	private const int MaxBricks = 50;
	private const float brickCreationIndex = 1.5f;
	private GameObject[,] bricks = new GameObject[6, 16];
	private Ball ball;

	public List<GameObject> prefabs;

	// Use this for initialization
	void Start () {
		ball = GameObject.FindObjectOfType<Ball>();

		for (int i = 0; i < MaxBricks; i++) {
			CreateRandomBrick ();
		}
	}

	void Update ()
	{
		float createBrickProbability = brickCreationIndex/(Brick.breakableCount + 1f) * Time.deltaTime/Time.maximumDeltaTime;

		if (Random.value < createBrickProbability)
		{
			CreateRandomBrick();
		}
	}
	
	public void CreateRandomBrick() {
		if (Brick.breakableCount < MaxBricks && ball.transform.position.y < 600) {
			int type = UnityEngine.Random.Range (0, prefabs.Count);
			int posX = UnityEngine.Random.Range (0, 6);
			int posY = UnityEngine.Random.Range (0, 16);
			int initialHits = UnityEngine.Random.Range (0, prefabs [type].GetComponent<Brick> ().skins.Count);

			CreateBrick (type, posX, posY, initialHits);
		}
	}

	private void CreateBrick(int type, int posX, int posY, int initialHits) {
		if (bricks [posX, posY] == null) {
			bricks [posX, posY] = Instantiate (prefabs[type]);
			bricks [posX, posY].transform.parent = transform;
			bricks [posX, posY].GetComponent<Brick> ().SetInitialHits (initialHits);

			Vector2 startPos = new Vector2 (150 * posX + 75, 50 * posY + 700);
			bricks [posX, posY].transform.position = startPos;
		}
	}
}
