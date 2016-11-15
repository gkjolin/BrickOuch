using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour {
	static MusicPlayer instance = null;
	
	// Use this for initialization
	void Start () {
		if (instance != null) {
			Destroy (gameObject);
		} else {
			instance = this;
			GameObject.DontDestroyOnLoad(gameObject);
			UpdateVolume(PlayerPrefsManager.GetMusicVolume());
		}
	}
	
	public void UpdateVolume(float volume)
	{
		var audioSource = gameObject.GetComponent<AudioSource>();

		audioSource.volume = volume;
	}

	public void ToggleMute()
	{
		var audioSource = gameObject.GetComponent<AudioSource>();

		if (audioSource.isPlaying)
		{
			audioSource.Pause();
		}
		else
		{
			audioSource.Play();
		}
	}
}
