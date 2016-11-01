﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using Spine.Unity;

public class Paddle : MonoBehaviour
{

	private const float SpeedFactor = 2000.0f;

	public bool autoPlay = false;
	public bool mousePlay = true;
	public bool gameOver = false;
	private float minX, maxX;

	private Ball ball;
	private PlaySpace playSpace;

	private SkeletonAnimation skeletonAnimation;
	private float lastPos;

	void Start ()
	{
		this.GetComponent<Collider2D> ().isTrigger = true;
		Screen.sleepTimeout = SleepTimeout.NeverSleep;

		ball = GameObject.FindObjectOfType<Ball> ();
		playSpace = GameObject.FindObjectOfType<PlaySpace> ();
		mousePlay = !SystemInfo.supportsAccelerometer;

		skeletonAnimation = this.GetComponent<SkeletonAnimation> ();
		lastPos = this.transform.position.x;

		float halfSizeX = this.GetComponent<BoxCollider2D> ().bounds.size.x / 2;
		minX = halfSizeX;
		maxX = playSpace.Width - halfSizeX;
	}
		
	// Update is called once per frame
	void Update ()
	{
		if (gameOver) {
			return;
		}
			
		if (autoPlay) {
			AutoPlay ();
		} else if (mousePlay) {
			MoveWithMouse ();
		} else {
			MoveWithAccelerometer ();
		}

		MoveAnimation ();
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
		skeletonAnimation.state.AddAnimation (0, "Hit", false, 0);
	}

	public void EndGameAnimation ()
	{
		gameOver = true;
		skeletonAnimation.state.ClearTracks ();
		skeletonAnimation.state.SetAnimation (0, "EndGame", false);
	}

	private void MoveAnimation ()
	{
		if (this.transform.position.x < lastPos) {
			skeletonAnimation.state.AddAnimation (1, "Tras", true, 0);
		} else if (this.transform.position.x > lastPos) {
			skeletonAnimation.state.AddAnimation (1, "Tras", true, 0);
		} else {
			skeletonAnimation.state.ClearTrack (1);
		}

		lastPos = this.transform.position.x;
	}

	public IEnumerator StartGameAnimation ()
	{
		skeletonAnimation.state.AddAnimation (0, "StartGame", false, 0);

		yield return new WaitForSeconds(1.2f);
		this.GetComponent<Collider2D> ().isTrigger = false;
	}

}
