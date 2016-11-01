using UnityEngine;
using System;
using System.Collections;

public class Background : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Vector2 worldMin = Camera.main.ScreenToWorldPoint (Vector2.zero);
		Vector2 worldMax = Camera.main.ScreenToWorldPoint (new Vector2 (Screen.width, Screen.height));
		Vector2 worldSize = worldMax - worldMin;

		Sprite sprite = this.GetComponent<SpriteRenderer> ().sprite;
		float visibleBackgroundWidth = sprite.bounds.size.x;
		float visibleBackgroundHeight = sprite.bounds.max.y;
		float scale = Math.Max (worldSize.x / visibleBackgroundWidth, worldSize.y / visibleBackgroundHeight);
		this.transform.localScale = new Vector2 (scale, scale);
	}
}
