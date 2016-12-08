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
      
}
