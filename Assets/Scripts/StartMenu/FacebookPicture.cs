using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Facebook.Unity;

public class FacebookPicture : MonoBehaviour {

	public Image facebookImage;
	public Image pictureImage;

	public Sprite facebookSprite;
	public Sprite pictureSprite;

	void Update ()
	{
		if (FB.IsLoggedIn) {
			SetLoggedInState ();
		} else {
			SetLoggedOutState ();
		}
	}

	public void SetLoggedInState () {
		if (FacebookAccess.Instance.Picture != null) {
			pictureImage.sprite = FacebookAccess.Instance.Picture;
			pictureImage.gameObject.SetActive (true);
		} else {
			pictureImage.gameObject.SetActive (false);
		}

		facebookImage.sprite = pictureSprite;
	}

	public void SetLoggedOutState () {
		facebookImage.sprite = facebookSprite;
		pictureImage.gameObject.SetActive (false);
	}
}
