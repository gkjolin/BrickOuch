﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using Spine.Unity;

public class Paddle : MonoBehaviour
{
	public bool autoPlay = false;
	public bool mousePlay = true;
	public bool freezePaddle = false;

	private float touchOffset;
	private float minX, maxX;
	private const float accelerometerSpeedFactor = 4000.0f;
	private const float accelerometerThreshold = 0.05f;
	private const int rotationAngleMax = 30;

	private Ball ball;
	private PlaySpace playSpace;

	private SkeletonAnimation skeletonAnimation;
	private Vector3 lastPos;

	void Start ()
	{
		this.GetComponent<Collider2D> ().isTrigger = true;
		Screen.sleepTimeout = SleepTimeout.NeverSleep;

		ball = GameObject.FindObjectOfType<Ball> ();
		mousePlay = PlayerPrefsManager.GetControllerMode() == Constants.HowToPlayModes.PlayWithMouse || !SystemInfo.supportsAccelerometer;

		skeletonAnimation = this.GetComponent<SkeletonAnimation> ();
		UpdateLastPos();

		float halfSizeX = this.GetComponent<BoxCollider2D> ().bounds.size.x / 2;
		minX = halfSizeX;
		maxX = PlaySpace.Width - halfSizeX;
	}
		
	// Update is called once per frame
	void Update ()
	{
		if (!freezePaddle) 
		{
			if (autoPlay) 
			{
				AutoPlay ();
			} 
			else if (mousePlay) 
			{
				MoveWithMouse (false);
			} 
			else 
			{
				MoveWithAccelerometer (false);
			}

			MoveAnimation ();
			UpdateLastPos ();
		}
	}

	public void Reset()
	{
		this.GetComponent<Collider2D> ().isTrigger = true;
	}

	void AutoPlay ()
	{
		Vector3 paddlePos = new Vector3 (0.5f, this.transform.position.y, 0f);
		Vector3 ballPos = ball.transform.position;
		paddlePos.x = Mathf.Clamp (ballPos.x, minX, maxX);
		this.transform.position = paddlePos;
	}

	void MoveWithMouse (bool rotatePaddle = true)
	{
		if (Input.GetMouseButtonDown (0)) {
			touchOffset = this.transform.position.x - Camera.main.ScreenToWorldPoint (Input.mousePosition).x;
		}

		if (Input.GetMouseButton (0)) {
			var mouseWorldPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			float cannonPosX = Mathf.Clamp (mouseWorldPosition.x + touchOffset, minX, maxX);
			if (cannonPosX == minX || cannonPosX == maxX) {
				touchOffset = cannonPosX - mouseWorldPosition.x;
			}

			if (rotatePaddle) {
				RotateWithAccelerometer ();
			}

			Vector2 paddlePos = new Vector2 (cannonPosX, this.transform.position.y);
			this.GetComponent<Rigidbody2D>().MovePosition(paddlePos);
		}
	}

	void MoveWithAccelerometer (bool rotatePaddle = true)
	{
		Vector3 speed = Vector3.zero;
		Vector3 pos = new Vector3 (0.5f, this.transform.position.y, 0f);

		speed.x = Input.acceleration.x;

		if (speed.sqrMagnitude > 1)
			speed.Normalize ();

		if (rotatePaddle)
		{
			RotateWithAccelerometer();
		}

		if (Mathf.Abs(speed.x) < accelerometerThreshold)
		{
			speed.x = Mathf.MoveTowards(speed.x, 0, Time.deltaTime);
		}

		speed *= Time.deltaTime;
		this.transform.Translate (speed * accelerometerSpeedFactor);
		pos.x = Mathf.Clamp (this.transform.position.x, minX, maxX);

		this.transform.position = pos;
	}

	private void RotateWithAccelerometer()
	{
		Vector3 speed = Vector3.zero;
		speed.x = Input.acceleration.x;
		this.transform.rotation = Quaternion.AngleAxis(speed.x * rotationAngleMax, Vector3.back);
	}

	void OnCollisionEnter2D (Collision2D collision) {
		skeletonAnimation.state.AddAnimation (2, "Hit", false, 0);
	}

	public void EndGameAnimation ()
	{
		freezePaddle = true;
		skeletonAnimation.state.ClearTracks ();
		skeletonAnimation.state.SetAnimation (0, "EndGame", false);
	}

	private void UpdateLastPos ()
	{
		lastPos = this.transform.position;
	}

	private void MoveAnimation ()
	{
		if (this.transform.position.x < lastPos.x) {
			skeletonAnimation.state.AddAnimation (1, "Tras", true, 0);
		} else if (this.transform.position.x > lastPos.x) {
			skeletonAnimation.state.AddAnimation (1, "Tras", true, 0);
		} else {
			skeletonAnimation.state.ClearTrack (1);
		}
	}

	public IEnumerator StartGameAnimation ()
	{
		skeletonAnimation.state.AddAnimation (0, "StartGame", false, 0);

		yield return new WaitForSeconds(1.2f);
		this.GetComponent<Collider2D> ().isTrigger = false;
	}

}
