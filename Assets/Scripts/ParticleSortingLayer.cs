using UnityEngine;
using System.Collections;

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
