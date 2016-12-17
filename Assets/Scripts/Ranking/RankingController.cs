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

	private bool updateFontSize = false;

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

	void Update ()
	{
		if (updateFontSize) {
			UpdateFontSize ();
			updateFontSize = false;
		}
	}

	private void UpdateFontSize ()
	{
		Text[] texts = GameObject.FindObjectsOfType<Text> ();
		int minFontSize = int.MaxValue;

		foreach (Transform row in rowContainer.transform) {
			var rankText = row.transform.Find ("Rank/Text").GetComponent<Text> ();
			var nameText = row.transform.Find ("Name/Text").GetComponent<Text> ();
			var scoreText = row.transform.Find ("Score/Text").GetComponent<Text> ();

			minFontSize = Mathf.Min (minFontSize, rankText.cachedTextGenerator.fontSizeUsedForBestFit);
			minFontSize = Mathf.Min (minFontSize, nameText.cachedTextGenerator.fontSizeUsedForBestFit);
			minFontSize = Mathf.Min (minFontSize, scoreText.cachedTextGenerator.fontSizeUsedForBestFit);
		}

		foreach (Transform row in rowContainer.transform) {
			var rankText = row.transform.Find ("Rank/Text").GetComponent<Text> ();
			var nameText = row.transform.Find ("Name/Text").GetComponent<Text> ();
			var scoreText = row.transform.Find ("Score/Text").GetComponent<Text> ();

			rankText.resizeTextForBestFit = false;
			rankText.fontSize = minFontSize;

			nameText.resizeTextForBestFit = false;
			nameText.fontSize = minFontSize;

			scoreText.resizeTextForBestFit = false;
			scoreText.fontSize = minFontSize;
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
		updateFontSize = true;

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

		var rankText = row.transform.Find ("Rank/Text").GetComponent<Text> ();
		var nameText = row.transform.Find ("Name/Text").GetComponent<Text> ();
		var scoreText = row.transform.Find ("Score/Text").GetComponent<Text> ();
		var pictureImage = row.transform.Find ("Picture").GetComponent<Image> ();

		rankText.text = rank.ToString();
		nameText.text = name;
		scoreText.text = score.ToString();

		if (picture != null) {
			pictureImage.sprite = picture;
		} else {
			pictureImage.sprite = defaultPicture;
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
