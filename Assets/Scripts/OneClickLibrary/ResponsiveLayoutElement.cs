using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResponsiveLayoutElement : MonoBehaviour {

	public RectTransform reference;
	public Vector2 percentage;
	public bool switchAxis;

	void Update () {
		var size = reference.rect.size;
		if (switchAxis) {
			size.Set (size.y, size.x);
		}

		size.Scale (percentage);

		var layoutElement = GetComponent<LayoutElement> ();
		if (layoutElement == null) {
			layoutElement = gameObject.AddComponent <LayoutElement> ();
		}

		if (percentage.x != 0) {
			layoutElement.preferredWidth = size.x;
		}

		if (percentage.y != 0) {
			layoutElement.preferredHeight = size.y;
		}
	}
}
