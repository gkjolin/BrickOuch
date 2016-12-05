using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Facebook.Unity;

public class LeaderboardController : MonoBehaviour
{

	//  Leaderboard
	public GameObject LeaderboardPanel;
	public GameObject LeaderboardItemPrefab;
	public ScrollRect LeaderboardScrollRect;
	public Text NotLoggedText;

	// Use this for initialization
	void Start ()
	{
		Debug.Log ("Leaderboard start");
		if (FB.IsLoggedIn) {
			NotLoggedText.gameObject.SetActive(false);
			Debug.Log ("Logged on Facebook");

			populateLeaderBoard ();
		} else {
			NotLoggedText.gameObject.SetActive(true);
			Debug.Log ("Not logged on Facebook");
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	private void populateLeaderBoard ()
	{
		var scores = FacebookAccess.Scores;
		Debug.Log ("Score count: "+ scores.Count);
		// Populate leaderboard
		for (int i = 0; i < scores.Count; i++) {
			GameObject LBgameObject = Instantiate (LeaderboardItemPrefab) as GameObject;
			LeaderboardElement LBelement = LBgameObject.GetComponent<LeaderboardElement> ();
			LBelement.SetupElement (i + 1, scores [i]);
			LBelement.transform.SetParent (LeaderboardPanel.transform, false);
		}

		// Scroll to top
		LeaderboardScrollRect.verticalNormalizedPosition = 1f;
	}
}
