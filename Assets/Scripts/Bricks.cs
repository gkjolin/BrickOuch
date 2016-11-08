using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class Bricks : MonoBehaviour {

	private const int MaxBricks = 50;
	private const float brickCreationIndex = 1.5f;
	private List<Brick> bricks = new List<Brick>();
	private Ball ball;
	private Paddle paddle;

	public List<GameObject> prefabs;

	// Use this for initialization
	void Start () {
		ball = GameObject.FindObjectOfType<Ball>();
		paddle = GameObject.FindObjectOfType<Paddle>();

		CreateMultipleBricks(MaxBricks);
	}

	void Update ()
	{
		if (bricks.Where(b => b != null).Count() == 0)
		{
        	GoToNextLevel();
		}
	}

	private void GoToNextLevel()
	{
		ball.Reset();
		paddle.Reset();
		CreateMultipleBricks(MaxBricks);
	}

	private void CreateBrickOverTime()
	{
		float createBrickProbability = brickCreationIndex/(Brick.breakableCount + 1f) * Time.deltaTime/Time.maximumDeltaTime;

		if (ball.HasBeenLaunched && Random.value < createBrickProbability)
		{
			CreateRandomBrick();
		}
	}

	private void CreateMultipleBricks(int quantity)
	{
		for (int i = 0; i < quantity; i++)
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

	private void CreateBrick(int type, int posX, int posY, int initialHits)
	{
		var pos = new Vector2(posX, posY);

		if (GetBrickAt(pos) == null) {
			var obj = Instantiate (prefabs[type]);
			var newBrick = obj.GetComponent<Brick>();

			newBrick.Position = pos;
			newBrick.gameObject.transform.parent = transform;
			newBrick.SetInitialHits (initialHits);

			Vector2 startPos = new Vector2 (150 * posX + 75, 50 * posY + 700);
			obj.transform.position = startPos;

			bricks.Add(newBrick);
		}
	}

	private Brick GetBrickAt(Vector2 position)
	{
		return bricks
			.Where(b => b != null)
			.Where(b => b.Position == position)
			.FirstOrDefault();
	}
}
