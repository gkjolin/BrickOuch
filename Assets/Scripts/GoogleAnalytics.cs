using UnityEngine;
using System.Collections;
using SA.Analytics.Google;

public class GoogleAnalytics : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Manager.StartTracking ();
	}
	
}
