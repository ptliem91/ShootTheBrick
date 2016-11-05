using UnityEngine;
using System.Collections;

public class SpawnBalls : MonoBehaviour
{

	private float startTime;
	private float nextSpawn = 0f;

	public float spawnRate = 1f;
	//	public float randomDelay = 2f;

	public Transform prefabToSpawn;

	void Start ()
	{
		startTime = 0f;
	}

	// Update is called once per frame
	void Update ()
	{
		if (Time.time > nextSpawn) {

			Vector3 orgPosition = transform.position;

			Vector3 temp = transform.position;
			temp.x += Random.Range (-1, 2);
			transform.position = temp;

			Instantiate (prefabToSpawn, transform.position, Quaternion.identity);

			nextSpawn = Time.time + spawnRate; //+ Random.Range (1, randomDelay)

			transform.position = orgPosition;

		} else {
			startTime++;
		}
	}
}
