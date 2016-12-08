using UnityEngine;
using System.Collections;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T> {

	public static T Instance { get; private set; }

	void Awake ()
	{
		if (Instance == null ) {
			Instance = (T)this;
			DontDestroyOnLoad (this);
			Initialize ();
		} else {
			Destroy (gameObject);
		}
	}

	protected abstract void Initialize ();

}
