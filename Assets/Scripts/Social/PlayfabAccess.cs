using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using System.Linq;

public class PlayfabAccess : Singleton<PlayfabAccess>
{
	public string Id { get; private set; }
	public List<PlayerLeaderboardEntry> Scores { get; private set; }
	public Dictionary <string, string> FriendsIds { get; private set; }

	protected override bool Destroyable {
		get {
			return false;
		}
	}

	protected override void Initialize ()
	{
		FriendsIds = new Dictionary<string, string> ();
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

		GetFriendsDetails ();
		GetUserData ();
	}

	private void LoginErrorCallback (PlayFabError error) {
		Debug.Log ("Error logging in player with custom ID:");
		Debug.Log (error.ErrorMessage);
	}

	private void GetFriendsDetails ()
	{
		GetFriendsListRequest request = new GetFriendsListRequest () {
			IncludeFacebookFriends = true,
			IncludeSteamFriends = false,
		};

		PlayFabClientAPI.GetFriendsList(request, FriendsDetailsSuccess, FriendsDetailsError);
	}

	private void FriendsDetailsSuccess (GetFriendsListResult result)
	{
		foreach (var friend in result.Friends) {
			var facebookId = friend.FacebookInfo.FacebookId;
			var playfabId = friend.FriendPlayFabId;

			FriendsIds[playfabId] = facebookId;
		}
	}

	private void FriendsDetailsError (PlayFabError error)
	{
		Debug.Log (error.ErrorMessage);
	}

	private void GetUserData ()
	{
		var reqParams = new GetPlayerCombinedInfoRequestParams ();
		reqParams.GetUserAccountInfo = true;

		var request = new GetPlayerCombinedInfoRequest () {
			PlayFabId = Id,
			InfoRequestParameters = reqParams
		};

		PlayFabClientAPI.GetPlayerCombinedInfo (request, (result) => {
			Debug.Log ("Got account info:");
			var accountInfo = result.InfoResultPayload.AccountInfo;

			if ((accountInfo == null)) {
				Debug.Log ("No account available");
			} else {
				UserFacebookInfo fbInfo = accountInfo.FacebookInfo;
				if (fbInfo == null) {
					Debug.Log ("No facebook info available");
				} else {
					FacebookAccess.Instance.Id = fbInfo.FacebookId;
					FacebookAccess.Instance.Name = fbInfo.FullName;

					this.UpdateDisplayName(FacebookAccess.Instance.Name);
				}
			}

			GetLeaderboard();
		}, (error) => {
			Debug.Log ("Got error retrieving user data:");
			Debug.Log (error.ErrorMessage);
		});
	}

	public void PostScore (int score)
	{
		if (PlayFabClientAPI.IsClientLoggedIn ()) {
			List<StatisticUpdate> stats = new List<StatisticUpdate> ();

			stats.Add (new StatisticUpdate () {
				StatisticName = Constants.ScoreKey,
				Value = score
			});

			stats.Add (new StatisticUpdate () {
				StatisticName = Constants.TotalScoreKey,
				Value = score
			});

			UpdatePlayerStatisticsRequest request = new UpdatePlayerStatisticsRequest ();
			request.Statistics = stats;

			PlayFabClientAPI.UpdatePlayerStatistics (request, (result) => {
				Debug.Log ("Success on update player score");
				GetLeaderboard();
			}, (error) => {
				Debug.Log ("Fail on update player score");
				GetLeaderboard();
			});
		} else {
			Debug.Log ("Must be logged in to update score");
		}
	}

	public void GetLeaderboard ()
	{
		if (PlayFabClientAPI.IsClientLoggedIn ()) {
			GetFriendLeaderboardRequest request = new GetFriendLeaderboardRequest () {
				StatisticName = Constants.TotalScoreKey,
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
		EventManager.Instance.RankingUpdate ();
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
