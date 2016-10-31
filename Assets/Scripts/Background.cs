using UnityEngine;
using System;
using System.Collections;

public class Background : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Sprite sprite = this.GetComponent<SpriteRenderer> ().sprite;
		Vector2 backgroundSize = sprite.bounds.size;

		Vector2 worldMin = Camera.main.ScreenToWorldPoint (Vector2.zero);
		Vector2 worldMax = Camera.main.ScreenToWorldPoint (new Vector2 (Screen.width, Screen.height));
		Vector2 worldSize = worldMax - worldMin;

		float scale = Math.Max (worldSize.x / backgroundSize.x, worldSize.y / backgroundSize.y);
		this.transform.localScale = new Vector2 (scale, scale);
	}
}
