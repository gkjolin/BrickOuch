using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using Spine.Unity;

public class Paddle : MonoBehaviour
{

	private const float SpeedFactor = 2000.0f;
	private const float AnimationFactor = 0.05f;

	public bool autoPlay = false;
	public bool mousePlay = true;
	private float minX, maxX;

	private Ball ball;
	private PlaySpace playSpace;

	private new SkeletonAnimation animation;
	private float moveAnimation = AnimationFactor;

	void Start ()
	{
		this.GetComponent<Collider2D> ().isTrigger = true;
		Screen.sleepTimeout = SleepTimeout.NeverSleep;

		ball = GameObject.FindObjectOfType<Ball> ();
		playSpace = GameObject.FindObjectOfType<PlaySpace> ();
		mousePlay = !SystemInfo.supportsAccelerometer;

		animation = this.GetComponent<SkeletonAnimation> ();

		float halfSizeX = this.GetComponent<BoxCollider2D> ().bounds.size.x / 2;
		minX = halfSizeX;
		maxX = playSpace.Width - halfSizeX;
	}
		
	// Update is called once per frame
	void Update ()
	{
		if (autoPlay) {
			AutoPlay ();
		} else if (mousePlay) {
			MoveWithMouse ();
		} else {
			MoveWithAccelerometer ();
		}
	}

	void AutoPlay ()
	{
		Vector3 paddlePos = new Vector3 (0.5f, this.transform.position.y, 0f);
		Vector3 ballPos = ball.transform.position;
		paddlePos.x = Mathf.Clamp (ballPos.x, minX, maxX);
		this.transform.position = paddlePos;
	}

	void MoveWithMouse ()
	{
		var mouseWorldPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		float mousePosX = Mathf.Clamp (mouseWorldPosition.x, minX, maxX);
		Vector3 paddlePos = new Vector3 (mousePosX, this.transform.position.y, 0f);

		MoveAnimation (paddlePos);
		this.transform.position = paddlePos;
	}

	void MoveWithAccelerometer ()
	{
		Vector3 speed = Vector3.zero;
		Vector3 pos = new Vector3 (0.5f, this.transform.position.y, 0f);

		speed.x = Input.acceleration.x;

		if (speed.sqrMagnitude > 1)
			speed.Normalize ();

		speed *= Time.deltaTime;
		this.transform.Translate (speed * SpeedFactor);
		pos.x = Mathf.Clamp (this.transform.position.x, minX, maxX);
		this.transform.position = pos;
	}

	void OnCollisionEnter2D (Collision2D collision) {
		animation.loop = false;
		animation.AnimationName = "Hit";
		moveAnimation = 0.3f;
	}

	public IEnumerator HitAnimation ()
	{

		yield return new WaitForSeconds(0.3f);
		animation.AnimationName = "";
	}

	public void MoveAnimation (Vector3 newPos)
	{
		moveAnimation -= Time.deltaTime;
		string newAnimation = "";

		if (newPos.x < this.transform.position.x) {
			newAnimation = "Frente";
		} else if (newPos.x > this.transform.position.x) {
			newAnimation = "Tras";
		} else {
			newAnimation = "";
		}

		if (newAnimation == animation.AnimationName) {
			moveAnimation = Math.Max(moveAnimation, AnimationFactor);
		} else if (moveAnimation < 0) {
			animation.loop = true;
			animation.AnimationName = newAnimation;
			moveAnimation = Math.Max(moveAnimation, AnimationFactor);
		}
	}

	public IEnumerator StartGameAnimation ()
	{
		animation.loop = false;
		animation.AnimationName = "StartGame";
		moveAnimation = 1.2f;

		yield return new WaitForSeconds(1.2f);
		this.GetComponent<Collider2D> ().isTrigger = false;
	}

}
