using UnityEngine;
using System.Collections;
using Spine.Unity;
using System.Linq;

public class Towers : MonoBehaviour {

	private SkeletonAnimation skeletonAnimation;
	private float collisionThreshold = 1f;

	void Start ()
	{
		skeletonAnimation = this.GetComponent<SkeletonAnimation> ();
	}

	void OnCollisionEnter2D (Collision2D collision)
	{
		var bounds = this.GetComponent<Collider2D>().bounds;
		var lastContactPoint = collision.contacts[0].point;

		if (IsApproximatelyTheSamePoint(lastContactPoint.x, bounds.min.x, collisionThreshold)) 
		{
			skeletonAnimation.state.SetAnimation(0, "hitLeft", false);
		}
		else if (IsApproximatelyTheSamePoint(lastContactPoint.x, bounds.max.x, collisionThreshold))
		{
			skeletonAnimation.state.SetAnimation(0, "hitRight", false);
		}
		else if (IsApproximatelyTheSamePoint(lastContactPoint.y, bounds.max.y, collisionThreshold))
		{
			skeletonAnimation.state.SetAnimation(0, "hitTop", false);
		}
	}

	private bool IsApproximatelyTheSamePoint(float x, float y, float threshold)
	{
		return Mathf.Abs(x - y) < threshold;
	}
}
