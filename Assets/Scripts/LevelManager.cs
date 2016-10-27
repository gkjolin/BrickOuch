using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour {

	public void LoadScene(string name){
		Brick.breakableCount = 0;
		SceneManager.LoadScene (name);
	}

	public void QuitRequest(){
		Application.Quit ();
	}
}
