using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScrollableSettings : MonoBehaviour {

	public int visibleCount;

	private float minY;
	private float maxY;

	void Awake() {
		var scroll = GetComponent <RectTransform> ();
		minY = scroll.anchorMin.y;
		maxY = scroll.anchorMax.y;
	}

	public void UpdateDimensions (int count) {
		var scroll = GetComponent <ScrollRect> ();
		var scrollRect = GetComponent<RectTransform> ();
		var content = scroll.content;

		UpdateScrollDimensions (scrollRect, count);
		UpdateContentDimensions (content, count);
	}

	private void UpdateScrollDimensions(RectTransform scroll, int count) {
		var scrollAreaSize = maxY - minY;
		var newScrollAreaSize = scrollAreaSize * (float)count / (float)visibleCount;
		var newMinY = maxY - newScrollAreaSize;

		if (newMinY < minY) {
			newMinY = minY;
		}

		scroll.anchorMin = new Vector2 (scroll.anchorMin.x, newMinY);
		scroll.sizeDelta = Vector2.zero;
	}

	private void UpdateContentDimensions(RectTransform content, int count) {
		var newMinY = 0f;

		if (count > visibleCount) {
			newMinY = 1f - (float)count / (float)visibleCount;
			content.anchorMin = new Vector2 (0, newMinY);
		}

		content.anchorMin = new Vector2 (0, newMinY);
		content.anchorMax = new Vector2 (1, 1);
		content.sizeDelta = Vector2.zero;
	}
}
