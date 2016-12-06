using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Facebook.Unity;

public class LeaderboardController : MonoBehaviour
{
	private const int LeaderboardAspectRatio = 5;

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
		int count = 1;
		foreach (var score in scores) {
			GameObject LBgameObject = Instantiate (LeaderboardItemPrefab) as GameObject;
			LeaderboardElement LBelement = LBgameObject.GetComponent<LeaderboardElement> ();
			LBelement.SetupElement (count, score.Key.Name, score.Value.Score);
			LBelement.transform.SetParent (LeaderboardPanel.transform, false);

			count++;
		}

		// Scroll to top
		LeaderboardScrollRect.verticalNormalizedPosition = 1f;

		UpdateLeaderboardHeight (scores.Count);
	}

	private void UpdateLeaderboardHeight (int count) {
		if (count == 0) {
			return;
		}

		AspectRatioFitter transform = LeaderboardPanel.GetComponent<AspectRatioFitter> ();
		transform.aspectRatio = LeaderboardAspectRatio / count;
	}
}
