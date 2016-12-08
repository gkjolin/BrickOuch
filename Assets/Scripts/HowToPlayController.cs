using UnityEngine;
using System.Collections;
using SA.Analytics.Google;

public class HowToPlayController : MonoBehaviour {

	public void SetPlayWithMouse()
	{
		PlayerPrefsManager.SetControllerMode(Constants.HowToPlayModes.PlayWithMouse);

		Manager.Client.SendEventHit ("howtoplay", "touch");
	}

	public void SetPlayWithAccelerometer()
	{
		PlayerPrefsManager.SetControllerMode(Constants.HowToPlayModes.PlayWithAccelerometer);

		Manager.Client.SendEventHit ("howtoplay", "accelerometer");
	}


		
}
