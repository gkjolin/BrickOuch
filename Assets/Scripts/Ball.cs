using UnityEngine;
using System.Collections;
using Spine.Unity;

public class Ball : MonoBehaviour {

	private const float Force = 1f;

	private Paddle paddle;
	private bool hasStarted = false;
	private Vector3 paddleToBallVector;

	private Rigidbody2D body;
	private new SkeletonAnimation animation;

	// Use this for initialization
	void Start () {
		paddle = GameObject.FindObjectOfType<Paddle>();
		paddleToBallVector = this.transform.position - paddle.transform.position;

		var audioSource = gameObject.GetComponent<AudioSource>();
		audioSource.volume = PlayerPrefsManager.GetSoundsVolume();

		body = this.GetComponent<Rigidbody2D> ();
		animation = this.GetComponent<SkeletonAnimation> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!hasStarted) {
			// Lock the ball relative to the paddle.
			this.transform.position = paddle.transform.position + paddleToBallVector;
			
			// Wait for a mouse press to launch.
			if (Input.GetMouseButtonDown (0)) {
				hasStarted = true;
				StartCoroutine (paddle.StartGameAnimation ());
				body.velocity = new Vector2 (-520f, 256f);
			}
		} else {
			body.AddForce (body.velocity.normalized * Force);
		}
	}
	
	void OnCollisionEnter2D (Collision2D collision) {
		if (hasStarted) {
			GetComponent<AudioSource>().Play();
		}
	}

	public void PuffAnimation()
	{
		body.velocity = Vector2.zero;
		animation.AnimationName = "Puff";
	}

}
