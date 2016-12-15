using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Facebook.Unity;

public class RankingController : MonoBehaviour
{
	public ScrollableSettings scroll;
	public Transform rowContainer;
	public GameObject rankingRowPrefab;

	void Start ()
	{
		Debug.Log ("Leaderboard start");

		if (FB.IsLoggedIn) {
			Debug.Log ("Logged on Facebook");
			PopulateLeaderBoard ();
		} else {
			Debug.Log ("Not logged on Facebook");
		}
	}

	private void PopulateLeaderBoard ()
	{
		var scores = PlayfabAccess.Instance.Scores;
		if (scores == null) {
			return;
		}

		Debug.Log ("Score count: " + scores.Count);

		foreach (var score in scores) {
			CreateRankingRow (score.Position + 1, score.DisplayName, score.StatValue);
		}

		scroll.UpdateDimensions ();
	}

	private void CreateRankingRow (int rank, string name, int score) {
		GameObject row = Instantiate (rankingRowPrefab) as GameObject;

		var rankText = row.transform.Find ("Rank/Text");
		var nameText = row.transform.Find ("Name/Text");
		var scoreText = row.transform.Find ("Score/Text");

		rankText.GetComponent<Text> ().text = rank.ToString();
		nameText.GetComponent<Text> ().text = name;
		scoreText.GetComponent<Text> ().text = score.ToString();

		row.transform.SetParent (rowContainer);
	}
}
