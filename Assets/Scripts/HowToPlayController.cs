using UnityEngine;
using System.Collections;
using SA.Analytics.Google;

public class HowToPlayController : MonoBehaviour {

	public void SetPlayWithMouse()
	{
		PlayerPrefsManager.SetControllerMode(Constants.HowToPlayModes.PlayWithMouse);

		GoogleAnalytics.HitAnalyticsEvent("howtoplay", "touch");
	}

	public void SetPlayWithAccelerometer()
	{
		PlayerPrefsManager.SetControllerMode(Constants.HowToPlayModes.PlayWithAccelerometer);

		GoogleAnalytics.HitAnalyticsEvent("howtoplay", "accelerometer");
	}


		
}
