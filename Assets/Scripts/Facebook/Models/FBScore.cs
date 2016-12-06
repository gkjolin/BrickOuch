using UnityEngine;
using System.Collections;

public class FBScore {

	public FBUser User { get; set; }
	public long Score { get; set; }

	public FBScore(FBUser user, long score) {
		this.User = user;
		this.Score = score;
	}

}
