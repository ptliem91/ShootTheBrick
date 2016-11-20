using UnityEngine;
using System.Collections;

/*
 Authored by Liempt - gaucuhanh - ptliem9119@gmail.com
 Copyright 2016-11-20
 */

public class DontDestroy : MonoBehaviour
{

	// Use this for initialization
	void Awake ()
	{
		DontDestroyOnLoad (gameObject);
	}
}
