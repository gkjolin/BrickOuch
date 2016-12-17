using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PlayFab.ClientModels;
using UnityEngine.UI;
using Facebook.Unity;

public class RankingController : MonoBehaviour
{
	public ScrollableSettings scrollSettings;
	public Transform rowContainer;
	public GameObject rankingRowPrefab;
	public Sprite defaultPicture;

	public GameObject facebookPanel;

	void Start ()
	{
		FacebookAccess.Instance.LoadFriendsPictures ();

		Debug.Log ("Leaderboard start");
		EventManager.Instance.OnRankingUpdate += RefreshRanking;

		if (FB.IsLoggedIn) {
			Debug.Log ("Logged on Facebook");
			DisableFacebookPanel ();
			RefreshRanking ();
		} else {
			Debug.Log ("Not logged on Facebook");
			EnableFacebookPanel ();
		}
	}

	private void RefreshRanking ()
	{
		var scores = PlayfabAccess.Instance.Scores;
		if (scores == null) {
			return;
		}

		Debug.Log ("Score count: " + scores.Count);

		ClearRanking ();
		PopulateRanking (scores);

		scrollSettings.UpdateDimensions (scores.Count);
	}

	private void ClearRanking () {
		foreach (Transform row in rowContainer.transform) {
			Destroy (row.gameObject);
		}
	}

	private void PopulateRanking (List<PlayerLeaderboardEntry> scores) {
		foreach (var score in scores) {
			Sprite picture = null;

			if (score.PlayFabId == PlayfabAccess.Instance.Id) {
				picture = FacebookAccess.Instance.Picture;
			} else if (PlayfabAccess.Instance.FriendsIds.ContainsKey (score.PlayFabId)) {
				var facebookId = PlayfabAccess.Instance.FriendsIds [score.PlayFabId];
				
				if (!string.IsNullOrEmpty(facebookId) && FacebookAccess.Instance.FriendsPictures.ContainsKey(facebookId)) {
					picture = FacebookAccess.Instance.FriendsPictures [facebookId];
				}
			}

			CreateRankingRow (picture, score.Position + 1, score.DisplayName, score.StatValue);
		}
	}

	private void CreateRankingRow (Sprite picture, int rank, string name, int score) {
		GameObject row = Instantiate (rankingRowPrefab) as GameObject;

		var rankText = row.transform.Find ("Rank/Text");
		var nameText = row.transform.Find ("Name/Text");
		var scoreText = row.transform.Find ("Score/Text");
		var pictureImage = row.transform.Find ("Picture");

		rankText.GetComponent<Text> ().text = rank.ToString();
		nameText.GetComponent<Text> ().text = name;
		scoreText.GetComponent<Text> ().text = score.ToString();

		if (picture != null) {
			pictureImage.GetComponent<Image> ().sprite = picture;
		} else {
			pictureImage.GetComponent<Image> ().sprite = defaultPicture;
		}

		row.transform.SetParent (rowContainer);
	}

	public void DisableFacebookPanel () {
		facebookPanel.SetActive (false);
	}

	public void EnableFacebookPanel () {
		facebookPanel.SetActive (true);
	}
}
