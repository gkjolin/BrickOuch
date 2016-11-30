using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LeaderboardElement : MonoBehaviour
{
	//   Element references (Set in Unity Editor)   //
	public Text Rank;
	public Text Name;
	public Text Score;

	public void SetupElement (int rank, object entryObj)
	{
		var entry = (Dictionary<string,object>)entryObj;
		var user = (Dictionary<string,object>)entry ["user"];

		Rank.text = rank.ToString () + ".";
		Name.text = ((string)user ["name"]).Split (new char[]{ ' ' }) [0];
		Score.text = "Smashed: " + FacebookAccess.GetScoreFromEntry (entry).ToString ();
	}
}
