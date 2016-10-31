using UnityEngine;
using System.Collections;
using Spine.Unity;

public class Brick : MonoBehaviour
{

	public static int breakableCount = 0;
	public int pointsWorth;
	public int maxHits;

	private int timesHit;
	private bool isBreakable;
	private Score score;
	private bool destroy;
	private SkeletonAnimation skeletonAnimation;
	
	// Use this for initialization
	void Start ()
	{
		score = GameObject.FindObjectOfType<Score> ();
		skeletonAnimation = this.GetComponent<SkeletonAnimation> ();

		isBreakable = (this.tag == "Breakable");
		// Keep track of breakable bricks
		if (isBreakable) {
			breakableCount++;
		}
		
		timesHit = 0;
	}

	void Update ()
	{
		if (destroy) {
			Destroy (gameObject);
		}
	}

	void OnCollisionEnter2D (Collision2D col)
	{
		if (isBreakable) {
			StartCoroutine (HandleHits ());
		}
	}

	IEnumerator HandleHits ()
	{
		timesHit++;

		if (timesHit >= maxHits) {
			breakableCount--;
			score.AddScore (pointsWorth);

			skeletonAnimation.AnimationName = "Pop";
			yield return new WaitForSeconds (0.5f);

			destroy = true;
		} else {
			skeletonAnimation.AnimationName = "";
			skeletonAnimation.AnimationName = "Bump";
		}
	}
}
