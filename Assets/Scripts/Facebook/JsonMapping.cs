using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab.ClientModels;

public class JsonMapping {

	// Score format
	// {
	//   "score": 4,
	//   "user": {
	//      "name": "Chris Lewis",
	//      "id": "10152646005463795"
	//   }
	// }
	public static FBScore GetScore (object json)
	{
		var dict = (Dictionary <string,object>) json;

		var userDict = (Dictionary<string,object>)dict ["user"];
		string userId = (string)userDict ["id"];
		string userName = (string)userDict ["name"];
		var scoreValue = Convert.ToInt64 (dict ["score"]);

		FBUser user = new FBUser (userId, userName);
		FBScore score = new FBScore (user, scoreValue);

		return score;
	}

	public static FBScore GetScore (PlayerLeaderboardEntry scoreEntry)
	{
		string userId = scoreEntry.PlayFabId;
		Debug.Log(userId);
		string userName = scoreEntry.DisplayName;
		Debug.Log(userName);
		var scoreValue = scoreEntry.StatValue;
		Debug.Log(scoreValue);

		FBUser user = new FBUser (userId, userName);
		FBScore score = new FBScore (user, scoreValue);

		return score;
	}

}
