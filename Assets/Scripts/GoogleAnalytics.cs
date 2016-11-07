using UnityEngine;
using System.Collections;
using SA.Analytics.Google;

public class GoogleAnalytics : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Manager.StartTracking ();
	}

	public static void HitAnalyticsEvent(string eventCategory, string eventAction)
	{
		Manager.Client.CreateHit(HitType.EVENT);
		Manager.Client.SetEventCategory(eventCategory);
		Manager.Client.SetEventAction(eventAction);
		Manager.Client.Send();
	}
	
}
