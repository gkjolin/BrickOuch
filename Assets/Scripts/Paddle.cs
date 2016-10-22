﻿using UnityEngine;
using System.Collections;

public class Paddle : MonoBehaviour {

	public bool autoPlay = false;
	public bool mousePlay = true;
	private float minX, maxX;
	public float speed = 35.0F;

	private Ball ball;
	
	void Start () {
		ball = GameObject.FindObjectOfType<Ball>();
		mousePlay = !SystemInfo.supportsAccelerometer;

		float halfSizeX = this.GetComponent<BoxCollider2D> ().bounds.size.x / 2;
		minX = halfSizeX;
		maxX = 900 - halfSizeX;
	}
		
	// Update is called once per frame
	void Update () {
		if (autoPlay) 
		{
			AutoPlay ();
		} 
		else if (mousePlay) 
		{
			MoveWithMouse ();
		} 
		else 
		{
			MoveWithAccelerometer();
		}
	}
	
	void AutoPlay() {
		Vector3 paddlePos = new Vector3 (0.5f, this.transform.position.y, 0f);
		Vector3 ballPos = ball.transform.position;
		paddlePos.x = Mathf.Clamp(ballPos.x, minX, maxX);
		this.transform.position = paddlePos;
	}
	
	void MoveWithMouse () {
		float mousePosX = Mathf.Clamp(Input.mousePosition.x / Screen.width * 900, minX, maxX);
		Vector3 paddlePos = new Vector3 (mousePosX, this.transform.position.y, 0f);
		this.transform.position = paddlePos;
	}

	void MoveWithAccelerometer ()
	{
        Vector3 dir = Vector3.zero;
        Vector3 pos = new Vector3 (0.5f, this.transform.position.y, 0f);

        dir.x = Input.acceleration.x;

        if (dir.sqrMagnitude > 1)
            dir.Normalize();
        
        dir *= Time.deltaTime;
        this.transform.Translate(dir * speed);
		pos.x = Mathf.Clamp(this.transform.position.x, minX, maxX);
		this.transform.position = pos;
	}
}
