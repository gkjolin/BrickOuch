using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using System.Linq;

public class PlayfabAccess : Singleton<PlayfabAccess>
{
	public string Id { get; private set; }

	public List<PlayerLeaderboardEntry> Scores { get; private set; }

	protected override bool Destroyable {
		get {
			return false;
		}
	}

	public void Login (string facebookToken)
	{
		LoginWithFacebookRequest request = new LoginWithFacebookRequest () {
			TitleId = PlayFabSettings.TitleId,
			AccessToken = facebookToken,
			CreateAccount = true
		};

		PlayFabClientAPI.LoginWithFacebook (request, LoginSuccessCallback, LoginErrorCallback);
	}

	private void LoginSuccessCallback (LoginResult result) {
		Id = result.PlayFabId;
		Debug.Log ("Got PlayFabID: " + Id);

		if (result.NewlyCreated) {
			Debug.Log ("(new account)");
		} else {
			Debug.Log ("(existing account)");
		}

		this.GetUserData ();
		this.GetLeaderboard();
	}

	private void LoginErrorCallback (PlayFabError error) {
		Debug.Log ("Error logging in player with custom ID:");
		Debug.Log (error.ErrorMessage);
	}

	private void GetUserData ()
	{
		GetAccountInfoRequest request = new GetAccountInfoRequest () {
			PlayFabId = Id
		};

		PlayFabClientAPI.GetAccountInfo (request, (result) => {
			Debug.Log ("Got account info:");
			if ((result.AccountInfo == null)) {
				Debug.Log ("No account available");
			} else {
				UserFacebookInfo fbInfo = result.AccountInfo.FacebookInfo;
				if (fbInfo == null) {
					Debug.Log ("No facebook info available");
				} else {
					FacebookAccess.Instance.Id = fbInfo.FacebookId;
					FacebookAccess.Instance.Name = fbInfo.FullName;

					this.UpdateDisplayName(FacebookAccess.Instance.Name);
				}

			}
		}, (error) => {
			Debug.Log ("Got error retrieving user data:");
			Debug.Log (error.ErrorMessage);
		});
	}

	public void PostScore (int score)
	{
		if (PlayFabClientAPI.IsClientLoggedIn ()) {
			List<StatisticUpdate> stats = new List<StatisticUpdate> ();

			stats.Add (new StatisticUpdate (){ StatisticName = "Score", Value = score });

			UpdatePlayerStatisticsRequest request = new UpdatePlayerStatisticsRequest ();
			request.Statistics = stats;

			PlayFabClientAPI.UpdatePlayerStatistics (request, OnUpdateUserStatisticsSuccess, OnUpdateUserStatisticFail);
		} else {
			Debug.Log ("Must be logged in to update score");
		}
	}

	private void OnUpdateUserStatisticsSuccess (UpdatePlayerStatisticsResult updateUserStatisticsResult)
	{
		Debug.Log ("Success on update player score");
	}

	private static void OnUpdateUserStatisticFail (PlayFabError playfab)
	{
		Debug.Log ("Fail on update player score");
	}

	public void GetLeaderboard ()
	{
		if (PlayFabClientAPI.IsClientLoggedIn ()) {
			GetFriendLeaderboardRequest request = new GetFriendLeaderboardRequest () {
				StatisticName = "Score",
				IncludeSteamFriends = false
			};

			PlayFabClientAPI.GetFriendLeaderboard (request, OnGetLeaderboardSuccess, OnGetLeaderboardFail);
		} else {
			Debug.Log ("Must be logged in to get leaderboard");
		}
	}

	private void OnGetLeaderboardSuccess (GetLeaderboardResult result)
	{
		Debug.Log ("Succeed to get leaderboard");

		HandleScoresData(result.Leaderboard);
	}

	private void OnGetLeaderboardFail (PlayFabError error)
	{
		Debug.Log ("Fail to get leaderboard");
		Debug.Log (error);
	}

	private void HandleScoresData (List<PlayerLeaderboardEntry> scoresResponse)
    {
		Scores = scoresResponse.OrderBy(s => s.Position).ToList();

    }

    public void UpdateDisplayName(string name)
    {
    	UpdateUserTitleDisplayNameRequest request = new UpdateUserTitleDisplayNameRequest(){
    		DisplayName = name
    	};

    	PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnUpdateUserTitleDisplayNameSuccess, OnUpdateUserTitleDisplayNameFail);
    }

	private void OnUpdateUserTitleDisplayNameSuccess (UpdateUserTitleDisplayNameResult result)
	{
		Debug.Log ("Succeed to update user diplay name");
	}

	private void OnUpdateUserTitleDisplayNameFail (PlayFabError error)
	{
		Debug.Log ("Fail to update user diplay name");
	}
}
