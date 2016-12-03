using UnityEngine;
using System.Collections;

public class PlaySpace : MonoBehaviour {

	public static float Width { get; set; }
	public static float Height { get; set; }
	public static float OffsetX { get; set; }
	public static float ScoreHeight { get; set; }

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
		var collider = this.GetComponentInChildren<Collider2D>();

		PlaySpace.Height = Camera.main.orthographicSize * 2;
		PlaySpace.OffsetX = collider.offset.x;
		// The width is the further x point minus twice the offset.x (one for the left side and another for the right side)
		PlaySpace.Width = collider.bounds.max.x - 2 * PlaySpace.OffsetX;
		PlaySpace.ScoreHeight = Height - this.GetComponentInChildren<Collider2D>().bounds.max.y;
	}
}
