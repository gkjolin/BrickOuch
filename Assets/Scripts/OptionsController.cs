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

	public void SaveAndApply()
	{
		if (musicSlider != null && musicPlayer != null)
		{
			PlayerPrefsManager.SetMusicVolume(musicSlider.value);
			musicPlayer.UpdateVolume();
		}

		if (soundsSlider != null && soundsSlider.value < 1)
		{
			PlayerPrefsManager.SetSoundsVolume(soundsSlider.value);
		}
	}

	public void Quit()
	{
		optionsPanel.SetActive(false);
	}
}
