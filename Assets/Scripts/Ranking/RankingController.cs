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
	public Text loadingText;

	public GameObject facebookPanel;

	private bool updateFontSize = false;

	void Start ()
	{
		Debug.Log ("Leaderboard start");

		EventManager.Instance.OnRankingUpdate += RefreshRanking;
		EventManager.Instance.OnFacebookLoginCancel += LoginCancelled;
		loadingText.gameObject.SetActive (false);

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

	void OnDestroy() {
		EventManager.Instance.OnRankingUpdate -= RefreshRanking;
		EventManager.Instance.OnFacebookLoginCancel -= LoginCancelled;
	}

	private void LoginCancelled ()
	{
		EnableFacebookPanel ();
		loadingText.gameObject.SetActive (false);
	}

	private void UpdateFontSize ()
	{
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

		loadingText.gameObject.SetActive (false);
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
		bool oddRow = true;
		Color oddColor = new Color (93f / 255f, 40f / 255f, 29f / 255f);
		Color evenColor = new Color (80f / 255f, 26f / 255f, 20f / 255f);

		foreach (var score in scores) {
			Sprite picture = null;
			Color color = oddRow ? oddColor : evenColor;
			oddRow = !oddRow;

			if (score.PlayFabId == PlayfabAccess.Instance.Id) {
				picture = FacebookAccess.Instance.Picture;
				color = new Color (150f / 255f, 30f / 255f, 39f / 255f);
			} else if (PlayfabAccess.Instance.FriendsIds.ContainsKey (score.PlayFabId)) {
				var facebookId = PlayfabAccess.Instance.FriendsIds [score.PlayFabId];
				
				if (!string.IsNullOrEmpty(facebookId) && FacebookAccess.Instance.FriendsPictures.ContainsKey(facebookId)) {
					picture = FacebookAccess.Instance.FriendsPictures [facebookId];
				}
			}

			CreateRankingRow (picture, score.Position + 1, score.DisplayName, score.StatValue, color);
		}
	}

	private void CreateRankingRow (Sprite picture, int rank, string name, int score, Color color) {
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

		var rankBackground = row.transform.Find ("Rank/Background").GetComponent<Image> ();
		var nameBackground = row.transform.Find ("Name/Background").GetComponent<Image> ();
		var scoreBackground = row.transform.Find ("Score/Background").GetComponent<Image> ();

		rankBackground.color = color;
		nameBackground.color = color;
		scoreBackground.color = color;

		row.transform.SetParent (rowContainer);
	}

	public void DisableFacebookPanel () {
		facebookPanel.SetActive (false);
	}

	public void EnableFacebookPanel () {
		facebookPanel.SetActive (true);
	}
}
