using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;

public class OptionsController : MonoBehaviour {

	public Slider musicSlider;
	public Slider soundsSlider;
	public GameObject optionsPanel;

	private MusicPlayer musicPlayer;

	void Start () {
		optionsPanel.SetActive(false);

		musicPlayer = GameObject.FindObjectOfType<MusicPlayer>();

		musicSlider.value = PlayerPrefsManager.GetMusicVolume();
		soundsSlider.value = PlayerPrefsManager.GetSoundsVolume();
	}

	public void SaveMusicVolume()
	{
		if (musicSlider != null && musicPlayer != null)
		{
			PlayerPrefsManager.SetMusicVolume(musicSlider.value);
			musicPlayer.UpdateVolume(musicSlider.value);
		}
	}

	public void SaveSoundsVolume()
	{
		if (soundsSlider != null)
		{
			PlayerPrefsManager.SetSoundsVolume(soundsSlider.value);
		}
	}

	public void Quit()
	{
		optionsPanel.SetActive(false);
	}
}
