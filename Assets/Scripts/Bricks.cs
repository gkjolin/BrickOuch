using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class Bricks : MonoBehaviour {

	public int BalloonsRows;
	public int BalloonsColumns; 
	public int MaxBricks;
	public SoundManager soundManager;
	public LevelManager levelManager;
	public AudioClip boingSound;
	public AudioClip popSound;

	public int BreakableCount { get; set; }

	private const float brickCreationIndex = 1.5f;
	private List<Brick> bricks = new List<Brick>();
	private Ball ball;
	private Paddle paddle;

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
		// Recover ball used to win the level
		paddle.IncrementLife ();
		levelManager.Phase++;

		this.Reset();
		ball.Reset(levelManager.Phase);
		paddle.Reset();

		CreateMultipleBricks(MaxBricks);
		GameObject.FindGameObjectWithTag("ScoreMultiplier").GetComponent<Text>().text = string.Format("x{0}", levelManager.Phase);
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
			int posX = UnityEngine.Random.Range (0, BalloonsColumns);
			int posY = UnityEngine.Random.Range (0, BalloonsRows);
			int maxHits = Mathf.Min(levelManager.Phase, prefabs[type].GetComponent<Brick>().skins.Count);
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
			newBrick.pointsWorth *= levelManager.Phase;

			var widthPerBalloon = PlaySpace.Width/BalloonsColumns;
			var heightPerBalloon = PlaySpace.UsefulPlaySpace / BalloonsRows / 2;

			Vector2 startPos = new Vector2 (PlaySpace.MinX + widthPerBalloon * (posX + 0.5f), PlaySpace.UsefulPlaySpace - heightPerBalloon * (posY + 0.5f) - obj.GetComponent<Collider2D>().offset.y);
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
