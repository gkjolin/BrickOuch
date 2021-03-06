﻿using UnityEngine;
using System.Collections;

public class EventManager : Singleton<EventManager> {

	public delegate void RankingUpdateAction ();
	public event RankingUpdateAction OnRankingUpdate;

	public delegate void FacebookLoginCancelAction ();
	public event FacebookLoginCancelAction OnFacebookLoginCancel;

	protected override bool Destroyable {
		get {
			return false;
		}
	}

	public void RankingUpdate() {
		if (OnRankingUpdate != null) {
			OnRankingUpdate ();
		}
	}

	public void FacebookLoginCancel() {
		if (OnFacebookLoginCancel != null) {
			OnFacebookLoginCancel ();
		}
	}
}
