using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;

public class FacebookPanelController : MonoBehaviour
{
	public Text welcomeText;
	public Button loginButton;

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
		FacebookAccess.Instance.Login ();
	}

	public void SetLoggedInState () {
		loginButton.GetComponentInChildren<Text> ().text = "Logout";
		welcomeText.text = "Welcome, " + FacebookAccess.Instance.Name;
	}

	public void SetLoggedOutState () {
		loginButton.GetComponentInChildren<Text> ().text = "Login";
		welcomeText.text = "Welcome";
	}
}
