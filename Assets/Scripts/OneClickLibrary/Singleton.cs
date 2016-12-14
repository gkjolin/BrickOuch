using UnityEngine;
using System.Collections;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T> {

	public static T Instance { get; private set; }

	protected abstract bool Destroyable { get; }

	void Awake ()
	{
		if (Instance == null ) {
			Instance = (T)this;
			Initialize ();

			if (!Destroyable) {
				DontDestroyOnLoad (this);
			}
		} else {
			Destroy (gameObject);
		}
	}

	protected virtual void Initialize () { }

}
