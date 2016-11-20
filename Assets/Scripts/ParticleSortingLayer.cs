using UnityEngine;
using System.Collections;

/*
 Authored by Liempt - gaucuhanh - ptliem9119@gmail.com
 Copyright 2016-11-20
 */

public class ParticleSortingLayer : MonoBehaviour
{
	public string sortingLayerName;
	public int sortingOrder;

	void Start ()
	{
		// Set the sorting layer of the particle system.
		GetComponent<ParticleSystem> ().GetComponent<Renderer> ().sortingOrder = sortingOrder;
	}
}
