using UnityEngine;
using System.Collections;

public class HowToPlayController : MonoBehaviour {

	public void SetPlayWithMouse()
	{
		PlayerPrefsManager.SetControllerMode(Constants.HowToPlayModes.PlayWithMouse);
	}

	public void SetPlayWithAccelerometer()
	{
		PlayerPrefsManager.SetControllerMode(Constants.HowToPlayModes.PlayWithAccelerometer);
	}
}
