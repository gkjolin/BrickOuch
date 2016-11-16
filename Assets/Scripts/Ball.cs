using UnityEngine;
using System.Collections;
using Spine.Unity;

public class Ball : MonoBehaviour {

	public float velocityMultiplier = 1f;
	public float minAngle = 25f;
	public SoundManager soundManager;
	public AudioClip hitSound;
	public AudioClip puffSound;

	public bool HasBeenLaunched { get; set; }

	private const float velocityIncreaseRate = 0.1f;

	private Paddle paddle;
	private Vector3 paddleToBallVector;
	private Rigidbody2D body;
	private SkeletonAnimation skeletonAnimation;

	// Use this for initialization
	void Start ()
	{
		// Prevent fast ball from scaping the playspace
		body = this.GetComponent<Rigidbody2D> ();
		body.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

		skeletonAnimation = this.GetComponent<SkeletonAnimation> ();

		paddle = GameObject.FindObjectOfType<Paddle>();
		paddleToBallVector = this.transform.position - paddle.transform.position;

		Reset(1);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!HasBeenLaunched)
		{
			// Lock the ball relative to the paddle.
			this.transform.position = paddle.transform.position + paddleToBallVector;
			
			// Wait for a mouse press to launch.
			if (Input.GetMouseButtonUp (0))
			{
				HasBeenLaunched = true;
				StartCoroutine (paddle.StartGameAnimation ());
				body.velocity = new Vector2 (-520f, 256f) * velocityMultiplier;
			}
		}
	}

	public void Reset (int phase)
	{
		HasBeenLaunched = false;
		body.velocity = Vector2.zero;
		velocityMultiplier = 1 + phase * velocityIncreaseRate;
		skeletonAnimation.state.ClearTracks ();
		skeletonAnimation.Skeleton.SetToSetupPose();
	}

	void OnCollisionEnter2D (Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Hittable")) {
			soundManager.PlaySound(hitSound);
		}
	}

	void OnCollisionExit2D(Collision2D collision)
	{
		body.velocity = ClampAngle(body.velocity, minAngle, 180 - minAngle);
	}

	private Vector2 ClampAngle(Vector2 velocity, float minAngle, float maxAngle)
	{
		float angle = Vector2.Angle(velocity, Vector2.left);
		float angleToRotate = (angle - Mathf.Clamp(angle, minAngle, maxAngle)) * velocity.y/Mathf.Abs(velocity.y);

		Vector2 finalDirection = Quaternion.Euler(0, 0, angleToRotate) * velocity;

		return finalDirection;
	}

	public void PuffAnimation()
	{
		body.velocity = Vector2.zero;
		skeletonAnimation.state.AddAnimation(1, "Puff", false, 0.5f);
		soundManager.PlaySound(puffSound);
	}

}
