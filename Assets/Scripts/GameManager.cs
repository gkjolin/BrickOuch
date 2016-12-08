using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;

public class GameManager : Singleton<GameManager> {

	private const float DigitSize = 120;

	public GameObject levelUpBackground;
	public GameObject levelUpNumber;

	public HashSet<FBScore> Scores { get; private set; }

	protected override void Initialize ()
	{
		Scores = new HashSet<FBScore> ();
	}

	void OnApplicationFocus (bool hasFocus) {
		if (hasFocus) {
			FacebookAccess.GetScores ();
		}
	}

	public void LevelUpAnimation(int level) {
		var levelStr = level.ToString ();
		var strSize = (levelStr.Length - 1) * DigitSize;
		var offset = -strSize / 2;

		foreach (var digit in levelStr) {
			CreateAnimation (levelUpNumber, digit.ToString (), offset, true);
			offset += DigitSize;
		}

		CreateAnimation (levelUpBackground);
	}

	private void CreateAnimation (GameObject prefab, string skin = null, float offset = 0, bool front = false)
	{
		var gameObj = Instantiate (prefab) as GameObject;
		gameObj.transform.Translate (offset, 0, front ? -5 : -4);

		var animation = gameObj.GetComponent<SkeletonAnimation> ();

		if (skin != null) {
			animation.skeleton.SetSkin (skin);
		}

		animation.state.AddAnimation (0, "Out", false, 2);
		DestroyOnComplete (animation);
	}

	private void DestroyOnComplete (SkeletonAnimation skeletonAnim)
	{
		skeletonAnim.state.End += delegate (Spine.AnimationState state, int trackIndex) {
			var animationName = state.GetCurrent(trackIndex).Animation.Name;

			if (animationName == "Out") {
				Destroy (skeletonAnim.gameObject);
			}
		};
	}
}
