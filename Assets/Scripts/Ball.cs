using UnityEngine;
using System.Collections;
using Spine.Unity;

public class Ball : MonoBehaviour {

	public float velocityMultiplier;
	public float angleControlMultiplier;
	public float minAngle;
	public SoundManager soundManager;
	public AudioClip hitSound;
	public AudioClip puffSound;
	public LoseCollider loseCollider;

	public bool HasBeenLaunched { get; set; }
	public bool ReadyToLaunch { get; set; }
	private Vector2? launchTouchPos = null;

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

		skeletonAnimation = this.GetComponent<SkeletonAnimation> ();

		paddle = GameObject.FindObjectOfType<Paddle>();
		paddleToBallVector = this.transform.position - paddle.transform.position;

		// Prevent from launching ball before level up animation ends
		Reset();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (HasBeenLaunched || LevelManager.Instance.GameIsPaused) {
			return;
		}

		// Lock the ball relative to the paddle.
		this.transform.position = paddle.transform.position + paddleToBallVector;

		if (ReadyToLaunch) {
			// Wait for a mouse press to launch.
			if (Input.GetMouseButtonDown (0)) {
				launchTouchPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			}

			if (Input.GetMouseButtonUp (0) && launchTouchPos.HasValue) {
				Vector2 releasePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);

				bool click = Vector3.Distance (launchTouchPos.Value, releasePos) < 10;
				bool insidePlaySpace = PlaySpace.Bounds.Contains (releasePos);

				if (click && insidePlaySpace) {
					HasBeenLaunched = true;
					paddle.DecrementLife ();
					StartCoroutine (paddle.StartGameAnimation ());
					body.velocity = new Vector2 (-520f, 256f) * velocityMultiplier;
				}

				launchTouchPos = null;
			}
		}
	}

	public void Reset ()
	{
		HasBeenLaunched = false;
		ReadyToLaunch = false;
		this.transform.position = paddle.transform.position + paddleToBallVector;
		body.velocity = Vector2.zero;
	}

	public void SetReadyToLaunch (int phase)
	{
		ReadyToLaunch = true;
		velocityMultiplier = 1 + phase * velocityIncreaseRate;
		skeletonAnimation.state.ClearTracks ();
		skeletonAnimation.Skeleton.SetToSetupPose();
	}

	void OnCollisionEnter2D (Collision2D collision)
	{

		if (collision.gameObject.CompareTag("Hittable")) {
			soundManager.PlaySound(hitSound);
		}

		Paddle paddle = collision.gameObject.GetComponent<Paddle> ();
		if (paddle != null && body.velocity.y > 0) {
			float distance = transform.position.x - paddle.transform.position.x;
			float rotationAngle = -distance * angleControlMultiplier;
			float currentAngle = Vector2.Angle(Vector2.right, body.velocity);
			float newAngle = currentAngle + rotationAngle;

			newAngle = Mathf.Min (newAngle, 180 - minAngle);
			newAngle = Mathf.Max (newAngle, minAngle);

			rotationAngle = newAngle - currentAngle;
			Quaternion rotation = Quaternion.AngleAxis (rotationAngle, Vector3.forward);
			body.velocity = rotation * body.velocity;
		}
	}

	void OnCollisionExit2D(Collision2D collision)
	{
		body.velocity = ClampAngle(body.velocity, minAngle, 180 - minAngle);
	}

	private Vector2 ClampAngle(Vector2 velocity, float minAngle, float maxAngle)
	{
		float angle = Vector2.Angle(velocity, Vector2.left);
		int direction = velocity.y >= 0 ? 1 : -1;
		float angleToRotate = (angle - Mathf.Clamp(angle, minAngle, maxAngle)) * direction;

		Vector2 finalDirection = Quaternion.Euler(0, 0, angleToRotate) * velocity;

		return finalDirection;
	}

	public void PuffAnimation()
	{
		body.velocity = Vector2.zero;
		skeletonAnimation.state.AddAnimation(1, "Puff", false, 0.5f);
		soundManager.PlaySound(puffSound, 10);
	}

}
