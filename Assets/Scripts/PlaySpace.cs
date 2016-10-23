using UnityEngine;
using System.Collections;

public class PlaySpace : MonoBehaviour {

	public float Width { get; set; }
	public float Height { get; set; }

	public float ActualWidth 
	{
		get { return Screen.height * Width / Height; }
	}
	public float ActualHeight 
	{
		get { return Screen.height; }
	}
	public float ScreenOffsetX 
	{
		get { return (Screen.width - ActualWidth) / 2; }
	}
	public float ShrinkRatio 
	{
		get { return Height / ActualHeight; }
	}

	// Use this for initialization
	void Start () {
		Height = Camera.main.orthographicSize * 2;
		Width = this.GetComponentInChildren<EdgeCollider2D>().bounds.size.x;
	}
}
