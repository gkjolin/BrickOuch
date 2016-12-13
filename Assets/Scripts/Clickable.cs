using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class Clickable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	private RectTransform rect;

	// Use this for initialization
	void Start () {
		rect = GetComponent<RectTransform> ();
		var button = GetComponent<Button> ();

		if (rect == null || button == null) {
			var message = "Clickable elements must hame a Button component. ";
			message += "Add a Button component to object: ";

			Debug.LogError (message + gameObject.name);
			Destroy (this);
			return;
		}

		if (rect.pivot.x != 0.5f || rect.pivot.y != 0.5f) {
			var message = "Clickable element will not animate correctly. ";
			message += "Pivot should be placed on center: ";

			Debug.LogWarning (message + gameObject.name);
		}
	}

	public void OnPointerDown (PointerEventData eventData) {
		rect.localScale = new Vector3 (0.9f, 0.9f, 1);
	}

	public void OnPointerUp (PointerEventData eventData) {
		rect.localScale = new Vector3 (1, 1, 1);
	}
}
