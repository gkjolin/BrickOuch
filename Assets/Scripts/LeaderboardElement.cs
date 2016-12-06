using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LeaderboardElement : MonoBehaviour
{
	//   Element references (Set in Unity Editor)   //
	public Text Rank;
	public Text Name;
	public Text Score;

	public void SetupElement (int rank, string name, long score)
	{
		Rank.text = rank.ToString () + ".";
		Name.text = string.IsNullOrEmpty (name) ? "" : name.Split (' ') [0];
		Score.text = "Smashed: " + score;
	}
}
