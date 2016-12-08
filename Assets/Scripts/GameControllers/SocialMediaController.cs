using UnityEngine;
using System.Collections;

public class SocialMediaController : MonoBehaviour
{

	public static SocialMediaController instance;

	public bool isLoggedIn;

	void Awake ()
	{
		MakeSingleton ();
	}

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void MakeSingleton ()
	{
		if (instance != null) {
			Destroy (gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad (gameObject);
		}
	}
}
