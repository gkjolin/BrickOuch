using UnityEngine;
using System.Collections;

public class PlaySpace : MonoBehaviour {

	public new Collider2D collider;

	public static float Width { get; set; }
	public static float Height { get; set; }
	public static float MinX { get; set; }
	public static float ScoreHeight { get; set; }
	public static Bounds Bounds { get; private set; }

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

	public static float UsefulPlaySpace 
	{
		get { return Height - ScoreHeight; }
	}

	// Use this for initialization
	void Start () {
		PlaySpace.Height = Camera.main.orthographicSize * 2;
		PlaySpace.MinX = collider.bounds.min.x;
		PlaySpace.Width = collider.bounds.size.x;
		PlaySpace.ScoreHeight = Height - collider.bounds.max.y;
		PlaySpace.Bounds = collider.bounds;
	}
}
