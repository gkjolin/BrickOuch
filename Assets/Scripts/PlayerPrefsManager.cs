﻿using UnityEngine;
using System.Collections;

public static class PlayerPrefsManager {

	private const string MUSIC_VOLUME_KEY = "music_volume";
	private const string SOUNDS_VOLUME_KEY = "sounds_volume";

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
}
