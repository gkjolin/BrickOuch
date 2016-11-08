﻿using UnityEngine;
using System.Collections;
using Spine.Unity;

public class Ball : MonoBehaviour {

	public bool HasBeenLaunched { get; set; }

	private const float velocityIncreaseRate = 0.1f;

	// This can be modified in the Editor for testing purpose
	public float velocityMultiplier = 1f;
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

		Reset(1);

		paddle = GameObject.FindObjectOfType<Paddle>();
		paddleToBallVector = this.transform.position - paddle.transform.position;

		var audioSource = gameObject.GetComponent<AudioSource>();
		audioSource.volume = PlayerPrefsManager.GetSoundsVolume();

		skeletonAnimation = this.GetComponent<SkeletonAnimation> ();
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

	public void Reset(int phase)
	{
		HasBeenLaunched = false;
		body.velocity = Vector2.zero;
		velocityMultiplier = 1 + phase * velocityIncreaseRate;
	}
	
	void OnCollisionEnter2D (Collision2D collision)
	{
		if (HasBeenLaunched)
		{
			GetComponent<AudioSource>().Play();
		}
	}

	public void PuffAnimation()
	{
		body.velocity = Vector2.zero;
		skeletonAnimation.AnimationName = "Puff";
	}

}
