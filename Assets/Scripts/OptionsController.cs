using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;

public class OptionsController : MonoBehaviour {

	private GameObject optionsPanel;
	private MusicPlayer musicPlayer;
	private Slider musicSlider;
	private Slider soundsSlider;

	void Start () {
		optionsPanel = GameObject.FindGameObjectWithTag("Options");
		optionsPanel.SetActive(false);

		musicPlayer = GameObject.FindObjectOfType<MusicPlayer>();

		var sliders = optionsPanel.GetComponentsInChildren<Slider>();
		musicSlider = sliders.FirstOrDefault(c => c.name.Equals("Music Slider"));
		soundsSlider = sliders.FirstOrDefault(c => c.name.Equals("Sounds Slider"));

		musicSlider.value = PlayerPrefsManager.GetMusicVolume();
		soundsSlider.value = PlayerPrefsManager.GetSoundsVolume();
	}

	public void SaveAndApply()
	{
		if (musicSlider != null)
		{
			PlayerPrefsManager.SetMusicVolume(musicSlider.value);
			musicPlayer.UpdateVolume();
		}

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
