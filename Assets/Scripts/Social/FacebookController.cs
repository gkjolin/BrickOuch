using UnityEngine;
using UnityEngine.UI;

public class FacebookController : MonoBehaviour
{
	public void FacebookInvite ()
	{
		FacebookAccess.Instance.Invite ();
	}

	public void  Login ()
	{
		FacebookAccess.Instance.Login ();
	}
}
