using UnityEngine;
using System.Collections;

public static class PlayerPrefsManager {

	private const string MUSIC_VOLUME_KEY = "music_volume";
	private const string SOUNDS_VOLUME_KEY = "sounds_volume";
	private const string HIGHEST_SCORE_KEY = "highest_score";
	private const string LAST_SCORE_KEY = "last_score";
	private const string CONTROLLER_MODE_KEY = "controller_mode_key";

	public static void SetMusicVolume(float volume)
	{
		if (volume >= 0 && volume <= 1)
		{
			PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, volume);
		}
		else
		{
			Debug.LogError("Trying to set an out of range volume");
		}
	}

	public static float GetMusicVolume()
	{
		return PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 1);
	}

	public static void SetSoundsVolume(float volume)
	{
		if (volume >= 0 && volume <= 1)
		{
			PlayerPrefs.SetFloat(SOUNDS_VOLUME_KEY, volume);
			Debug.LogError(PlayerPrefs.GetFloat(SOUNDS_VOLUME_KEY));
		}
		else
		{
			Debug.LogError("Trying to set an out of range volume");
		}
	}

	public static float GetSoundsVolume()
	{
		return PlayerPrefs.GetFloat(SOUNDS_VOLUME_KEY, 1);
	}

	public static void SetHighestScore(int score)
	{
		PlayerPrefs.SetInt(HIGHEST_SCORE_KEY, score);
		PlayerPrefs.Save();
	}

	public static int GetHighestScore()
	{
		return PlayerPrefs.GetInt(HIGHEST_SCORE_KEY, 0);
	}

	public static void SetControllerMode(Constants.HowToPlayModes mode)
	{
		PlayerPrefs.SetInt(CONTROLLER_MODE_KEY, (int)mode);
		PlayerPrefs.Save();
	}

	public static void SetLastScore(int score)
	{
		PlayerPrefs.SetInt(LAST_SCORE_KEY, score);
		PlayerPrefs.Save();
	}

	public static int GetLastScore()
	{
		return PlayerPrefs.GetInt(LAST_SCORE_KEY, 0);
	}

	public static Constants.HowToPlayModes GetControllerMode()
	{
		return (Constants.HowToPlayModes)PlayerPrefs.GetInt(CONTROLLER_MODE_KEY, 0);
	}
}
