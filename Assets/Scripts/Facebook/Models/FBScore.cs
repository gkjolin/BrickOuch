using UnityEngine;
using System.Collections;

public class FBScore {

	public FBUser User { get; set; }
	public long Score { get; set; }

	public FBScore(FBUser user, long score) {
		this.User = user;
		this.Score = score;
	}

	public override bool Equals (object obj)
	{
		if (obj is FBScore) {
			FBScore other = (FBScore)obj;
			return User.Equals (other.User);
		}

		return false;
	}

	public override int GetHashCode ()
	{
		return User.GetHashCode ();
	}

}
