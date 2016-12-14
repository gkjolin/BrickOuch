using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;

public class FacebookPanelController : MonoBehaviour
{
	public Text welcomeText;
	public Button loginButton;

	public Image facebookIcon;
	public Image facebookPicture;

	public Sprite facebookLogo;
	public Sprite userPicture;

	void Update ()
	{
		if (FB.IsLoggedIn) {
			SetLoggedInState ();
		} else {
			SetLoggedOutState ();
		}
	}

	public void FacebookInvite ()
	{
		FacebookAccess.Instance.Invite ();
	}

	public void  Login ()
	{
		FacebookAccess.Instance.Login (facebookPicture);
	}

	public void SetLoggedInState () {
		loginButton.GetComponentInChildren<Text> ().text = "Logout";
		welcomeText.text = "Welcome, " + FacebookAccess.Instance.Name;

		facebookIcon.sprite = userPicture;
	}

	public void SetLoggedOutState () {
		loginButton.GetComponentInChildren<Text> ().text = "Login";
		welcomeText.text = "Welcome";

		facebookIcon.sprite = facebookLogo;
		facebookPicture.gameObject.SetActive (false);
	}
}
