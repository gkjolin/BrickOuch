using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class Bricks : MonoBehaviour {

	public int BreakableCount { get; set; }

	private const int MaxBricks = 50;
	private const float brickCreationIndex = 1.5f;
	private List<Brick> bricks = new List<Brick>();
	private Ball ball;
	private Paddle paddle;
	private int phase = 1;

	public List<GameObject> prefabs;

	// Use this for initialization
	void Start () {
		BreakableCount = 0;
		ball = GameObject.FindObjectOfType<Ball>();
		paddle = GameObject.FindObjectOfType<Paddle>();
		CreateMultipleBricks(MaxBricks);
	}

	public void CheckLevelEnd()
	{
		if (BreakableCount == 0)
		{
			GoToNextLevel();
		}
	}

	public void Reset()
	{
		BreakableCount = 0;
		bricks = new List<Brick>();
	}

	private void GoToNextLevel()
	{
		phase++;
		this.Reset();
		ball.Reset(phase);
		paddle.Reset();
		CreateMultipleBricks(MaxBricks);
		GameObject.FindGameObjectWithTag("ScoreMultiplier").GetComponent<Text>().text = string.Format("x{0}", phase);
	}

	private void CreateBrickOverTime()
	{
		float createBrickProbability = brickCreationIndex/(BreakableCount + 1f) * Time.deltaTime/Time.maximumDeltaTime;

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
		if (BreakableCount < MaxBricks) {
			int type = UnityEngine.Random.Range (0, prefabs.Count);
			int posX = UnityEngine.Random.Range (0, 6);
			int posY = UnityEngine.Random.Range (0, 16);
			int maxHits = Mathf.Min(phase, prefabs[type].GetComponent<Brick>().skins.Count);
			int initialHits = UnityEngine.Random.Range(0, maxHits) + 1;

			CreateBrick (type, posX, posY, initialHits);
		}
	}

	private void CreateBrick(int type, int posX, int posY, int hp)
	{
		var pos = new Vector2(posX, posY);

		if (GetBrickAt(pos) == null) {
			var obj = Instantiate (prefabs[type]);
			var newBrick = obj.GetComponent<Brick>();

			newBrick.Position = pos;
			newBrick.gameObject.transform.parent = transform;
			newBrick.SetInitialHitPoints (hp);
			newBrick.pointsWorth *= phase;

			Vector2 startPos = new Vector2 (150 * posX + 75, 50 * posY + 700);
			obj.transform.position = startPos;

			bricks.Add(newBrick);

			if (newBrick.CompareTag("Breakable")) {
				BreakableCount++;
			}
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
