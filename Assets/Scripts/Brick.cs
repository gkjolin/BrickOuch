using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;

public class Brick : MonoBehaviour
{

	public static int breakableCount = 0;
	public int pointsWorth;
	public List<string> skins;

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

		skeletonAnimation.skeleton.SetSkin (skins [timesHit]);
		skeletonAnimation.state.End += delegate (Spine.AnimationState state, int trackIndex) {
			if (timesHit < skins.Count) {
				skeletonAnimation.skeleton.SetSkin (skins [timesHit]);
			} else {
				skeletonAnimation.skeleton.SetSkin (skins [skins.Count - 1]);
			}

			if (skeletonAnimation.AnimationName == "Pop") {
				Destroy (gameObject);
			}
		};
	}

	void OnCollisionEnter2D (Collision2D col)
	{
		if (isBreakable) {
			HandleHits ();
		}
	}

	private void HandleHits ()
	{
		timesHit++;

		if (timesHit >= skins.Count) {
			breakableCount--;
			score.AddScore (pointsWorth);

			Destroy (this.GetComponent<BoxCollider2D> ());
			skeletonAnimation.state.SetAnimation (0, "Pop", false);
		} else {
			skeletonAnimation.state.SetAnimation (0, "Bump", false);
		}
	}

	public void SetInitialHits(int initialHits)
	{
		this.timesHit = initialHits;
	}
}
