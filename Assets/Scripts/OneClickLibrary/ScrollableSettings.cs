using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScrollableSettings : MonoBehaviour {

	public int visibleCount;

	public void UpdateDimensions () {
		var scroll = GetComponent <ScrollRect> ();
		var scrollRect = scroll.GetComponent<RectTransform> ();
		var content = scroll.content;

		if (content.childCount < visibleCount) {
			var minY = scrollRect.anchorMin.y;
			var maxY = scrollRect.anchorMax.y;

			var scrollAreaSize = maxY - minY;
			var newScrollAreaSize = scrollAreaSize * (float)content.childCount / (float)visibleCount;
			var newMinY = maxY - newScrollAreaSize;

			scrollRect.anchorMin = new Vector2 (scrollRect.anchorMin.x, newMinY);
			scrollRect.sizeDelta = Vector2.zero;
		} else {
			var minY = 1f - (float)content.childCount / (float)visibleCount;
			content.anchorMin = new Vector2 (0, minY);
			content.sizeDelta = Vector2.zero;
		}
	}
}
