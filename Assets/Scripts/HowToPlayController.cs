using UnityEngine;
using System.Collections;
using SA.Analytics.Google;

public class HowToPlayController : MonoBehaviour {

	public void SetPlayWithMouse()
	{
		PlayerPrefsManager.SetControllerMode(Constants.HowToPlayModes.PlayWithMouse);


		HitAnalyticsEvent("touch");
	}

	public void SetPlayWithAccelerometer()
	{
		PlayerPrefsManager.SetControllerMode(Constants.HowToPlayModes.PlayWithAccelerometer);

		HitAnalyticsEvent("accelerometer");
	}

	void HitAnalyticsEvent(string eventAction)
	{
		Manager.Client.CreateHit(HitType.EVENT);
		Manager.Client.SetEventCategory("howtoplay");
		Manager.Client.SetEventAction(eventAction);
		Manager.Client.Send();
	}
		
}
