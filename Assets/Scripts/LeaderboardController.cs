using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Facebook.Unity;

public class LeaderboardController : MonoBehaviour
{
	private const int InvestmentAspectRatio = 5;

	//  Leaderboard
	public GameObject LeaderboardPanel;
	public GameObject LeaderboardItemPrefab;
	public ScrollRect LeaderboardScrollRect;
	public Text NotLoggedText;

	void OnEnable ()
	{
		Debug.Log ("Leaderboard start");

		if (FB.IsLoggedIn) {
			NotLoggedText.gameObject.SetActive(false);
			Debug.Log ("Logged on Facebook");

			PopulateLeaderBoard ();
		} else {
			NotLoggedText.gameObject.SetActive(true);
			Debug.Log ("Not logged on Facebook");
		}
	}

	private void PopulateLeaderBoard ()
	{
		var scores = FacebookAccess.Scores;
		Debug.Log ("Score count: " + scores.Count);

		// Clear out previous leaderboard
		Transform[] scoreElements = LeaderboardPanel.GetComponentsInChildren<Transform>();
		foreach(Transform childObject in scoreElements)
		{
			if (!LeaderboardPanel.transform.IsChildOf(childObject.transform))
			{
				Destroy (childObject.gameObject);
			}
		}

		// Populate leaderboard
		for (int i = 0; i < scores.Count; i++) {
			GameObject LBgameObject = Instantiate (LeaderboardItemPrefab) as GameObject;
			LeaderboardElement LBelement = LBgameObject.GetComponent<LeaderboardElement> ();
			LBelement.SetupElement (i + 1, scores [i]);
			LBelement.transform.SetParent (LeaderboardPanel.transform, false);
		}

		// Scroll to top
		LeaderboardScrollRect.verticalNormalizedPosition = 1f;

		UpdateLeaderboardHeight (scores.Count);
	}

	private void UpdateLeaderboardHeight (int count) {
		AspectRatioFitter transform = LeaderboardPanel.GetComponent<AspectRatioFitter> ();
		transform.aspectRatio = InvestmentAspectRatio / count;
	}
}
