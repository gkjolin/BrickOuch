using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;

public static class PlayfabAccess
{

	

	public static void PostScore (int score)
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

	private static void OnUpdateUserStatisticsSuccess (UpdatePlayerStatisticsResult updateUserStatisticsResult)
	{
		Debug.Log ("Success on update player score");
	}

	private static void OnUpdateUserStatisticFail (PlayFabError playfab)
	{
		Debug.Log ("Fail on update player score");
	}

	public static void GetLeaderboard ()
	{
		GetFriendLeaderboardRequest request = new GetFriendLeaderboardRequest () {
			StatisticName = "Score",
			IncludeSteamFriends = false
		};

		PlayFabClientAPI.GetFriendLeaderboard (request, OnGetLeaderboardSuccess, OnGetLeaderboardFail);
	}

	private static void OnGetLeaderboardSuccess (GetLeaderboardResult result)
	{
		Debug.Log ("Succeed to get leaderboard");

		HandleScoresData(result.Leaderboard);
	}

	private static void OnGetLeaderboardFail (PlayFabError error)
	{
		Debug.Log ("Fail to get leaderboard");
		Debug.Log (error);
	}

	private static void HandleScoresData (List<PlayerLeaderboardEntry> scoresResponse)
    {
		foreach(PlayerLeaderboardEntry scoreItem in scoresResponse) 
		{
			FBScore score = JsonMapping.GetScore (scoreItem);
			GameManager.Instance.Scores.Add (score);
        }
    }

    public static void UpdateDisplayName(string name)
    {
    	UpdateUserTitleDisplayNameRequest request = new UpdateUserTitleDisplayNameRequest(){
    		DisplayName = name
    	};

    	PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnUpdateUserTitleDisplayNameSuccess, OnUpdateUserTitleDisplayNameFail);
    }

	private static void OnUpdateUserTitleDisplayNameSuccess (UpdateUserTitleDisplayNameResult result)
	{
		Debug.Log ("Succeed to update user diplay name");
	}

	private static void OnUpdateUserTitleDisplayNameFail (PlayFabError error)
	{
		Debug.Log ("Fail to update user diplay name");
	}
}
