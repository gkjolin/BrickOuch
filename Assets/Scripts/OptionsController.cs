using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;

public class OptionsController : MonoBehaviour {

	private GameObject optionsPanel;
	private Slider musicSlider;
	private Slider soundsSlider;

	void Start () {
		optionsPanel = GameObject.FindGameObjectWithTag("Options");
		optionsPanel.SetActive(false);

		var sliders = optionsPanel.GetComponentsInChildren<Slider>();
		musicSlider = sliders.FirstOrDefault(c => c.name.Equals("Music Slider"));
		soundsSlider = sliders.FirstOrDefault(c => c.name.Equals("Sounds Slider"));

		musicSlider.value = PlayerPrefsManager.GetMusicVolume();
		soundsSlider.value = PlayerPrefsManager.GetSoundsVolume();
	}

	public void SaveAndApply()
	{
		PlayerPrefsManager.SetMusicVolume(musicSlider.value);
		PlayerPrefsManager.SetSoundsVolume(soundsSlider.value);
	}

	public void Quit()
	{
		optionsPanel.SetActive(false);
	}
}
