using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour
{

	public void LoadScene (string name)
	{
		Brick.breakableCount = 0;
		SceneManager.LoadScene (name);
	}

	public void QuitRequest ()
	{
		Application.Quit ();
	}

	public void PauseGame(){
		if (Time.timeScale == 0) {
			Time.timeScale = 1f;
		} else {
			Time.timeScale = 0;
		}
	}


}
