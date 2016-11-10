using UnityEngine;
using System.Collections;

public class PlaySpace : MonoBehaviour {

	public static float Width { get; set;}
	public static float Height { get; set;}

	public static float ActualWidth 
	{
		get { return Screen.height * Width / Height; }
	}
	public static float ActualHeight 
	{
		get { return Screen.height; }
	}
	public static float ScreenOffsetX 
	{
		get { return (Screen.width - ActualWidth) / 2; }
	}
	public static float ShrinkRatio 
	{
		get { return Height / ActualHeight; }
	}

	public static float ScoreHeight 
	{
		get { return Height * 0.14f; }
	}

	public static float UsefulPlaySpace 
	{
		get { return Height - ScoreHeight; }
	}

	// Use this for initialization
	void Start () {
		PlaySpace.Height = Camera.main.orthographicSize * 2;
		PlaySpace.Width = this.GetComponentInChildren<EdgeCollider2D>().bounds.size.x;
	}
}
