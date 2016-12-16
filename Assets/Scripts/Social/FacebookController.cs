using UnityEngine;
using UnityEngine.UI;

public class FacebookController : MonoBehaviour
{
	public void Invite ()
	{
		FacebookAccess.Instance.Invite ();
	}

	public void  Login ()
	{
		FacebookAccess.Instance.Login ();
	}
}
