using UnityEngine;
using System.Collections;

public class FBUser {

	public string Id { get; set; }
	public string Name { get; set; }

	public FBUser(string id, string name) {
		this.Id = id;
		this.Name = name;
	}

	public override bool Equals (object obj)
	{
		if (obj is FBUser) {
			FBUser other = (FBUser)obj;
			return Id.Equals (other.Id);
		}

		return false;
	}

	public override int GetHashCode ()
	{
		return Id.GetHashCode ();
	}
}
