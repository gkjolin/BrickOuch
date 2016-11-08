using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;

public class Brick : MonoBehaviour
{
	public int pointsWorth;
	public List<string> skins;

	public Vector2 Position { get; set; }

	private int hitPoints = 1;
	private Score score;
	private bool destroy;
	private SkeletonAnimation skeletonAnimation;

	// Use this for initialization
	void Start ()
	{
		score = GameObject.FindObjectOfType<Score> ();
		skeletonAnimation = this.GetComponent<SkeletonAnimation> ();

		Position = new Vector2();

		skeletonAnimation.skeleton.SetSkin (skins [hitPoints-1]);
		skeletonAnimation.state.End += delegate (Spine.AnimationState state, int trackIndex) {
			if (hitPoints > 0) {
				skeletonAnimation.skeleton.SetSkin (skins[hitPoints-1]);
			} else {
				skeletonAnimation.skeleton.SetSkin (skins[0]);
			}

			if (skeletonAnimation.AnimationName == "Pop") {
				Destroy (gameObject);
			}
		};
	}

	void OnCollisionEnter2D (Collision2D col)
	{
		if (this.CompareTag("Breakable"))
		{
			HandleHits ();
		}
	}

	private void HandleHits ()
	{
		hitPoints--;

		if (hitPoints <= 0)
		{
			var bricks = this.GetComponentInParent<Bricks>();

			bricks.BreakableCount--;
			score.AddScore (pointsWorth);

			Destroy (this.GetComponent<BoxCollider2D> ());
			skeletonAnimation.state.SetAnimation (0, "Pop", false);

			bricks.CheckLevelEnd();
		} 
		else 
		{
			skeletonAnimation.state.SetAnimation (0, "Bump", false);
		}
	}

	public void SetInitialHitPoints(int hp)
	{
		this.hitPoints = hp;
	}
}
