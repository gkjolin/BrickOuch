using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Spine.Unity;

public class Paddle : MonoBehaviour {

	private const float SpeedFactor = 2000.0f;

	public bool autoPlay = false;
	public bool mousePlay = true;
	private float minX, maxX;

	private Ball ball;
	private PlaySpace playSpace;

	private new SkeletonAnimation animation;
	
	void Start () {
		Screen.sleepTimeout = SleepTimeout.NeverSleep;

		ball = GameObject.FindObjectOfType<Ball>();
		playSpace = GameObject.FindObjectOfType<PlaySpace>();
		mousePlay = !SystemInfo.supportsAccelerometer;

		animation = this.GetComponent<SkeletonAnimation> ();

		float halfSizeX = this.GetComponent<BoxCollider2D> ().bounds.size.x / 2;
		minX = halfSizeX;
		maxX = playSpace.Width - halfSizeX;
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
		var mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		float mousePosX = Mathf.Clamp(mouseWorldPosition.x, minX, maxX);
		Vector3 paddlePos = new Vector3 (mousePosX, this.transform.position.y, 0f);

		this.transform.position = paddlePos;
	}

	void MoveWithAccelerometer ()
	{
		Vector3 speed = Vector3.zero;
		Vector3 pos = new Vector3 (0.5f, this.transform.position.y, 0f);

		speed.x = Input.acceleration.x;

		if (speed.sqrMagnitude > 1)
			speed.Normalize();

		speed *= Time.deltaTime;
		this.transform.Translate(speed * SpeedFactor);
		pos.x = Mathf.Clamp(this.transform.position.x, minX, maxX);
		this.transform.position = pos;
	}

	public void StartGameAnimation() {
		DisableCollider ();
		animation.loop = false;
		animation.AnimationName = "StartGame";
		Invoke ("EnableCollider", 1);
	}

	private void DisableCollider()
	{
		this.GetComponent<Collider2D> ().isTrigger = true;
	}

	private void EnableCollider()
	{
		this.GetComponent<Collider2D> ().isTrigger = false;
	}
}
