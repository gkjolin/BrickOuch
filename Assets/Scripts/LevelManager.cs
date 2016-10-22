using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour {

	public void LoadScene(string name){
		Debug.Log ("New Level load: " + name);
		Brick.breakableCount = 0;
		SceneManager.LoadScene (name);
	}

	public void QuitRequest(){
		Debug.Log ("Quit requested");
		Application.Quit ();
	}
}
