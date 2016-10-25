using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour {
	static MusicPlayer instance = null;
	
	// Use this for initialization
	void Start () {
		if (instance != null) {
			Destroy (gameObject);
			print ("Duplicate music player self-destructing!");
		} else {
			instance = this;
			GameObject.DontDestroyOnLoad(gameObject);
		}
	}
	
	public void UpdateVolume()
	{
		var audioSource = gameObject.GetComponent<AudioSource>();

		audioSource.volume = PlayerPrefsManager.GetMusicVolume();
	}
}
