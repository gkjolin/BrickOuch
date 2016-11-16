using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	private AudioSource audioSource;

	// Use this for initialization
	void Start () {
		audioSource = gameObject.GetComponent<AudioSource>();
		audioSource.volume = PlayerPrefsManager.GetSoundsVolume();
	}
	
	public void PlaySound(AudioClip sound)
	{
		audioSource.PlayOneShot(sound);
	}
}
