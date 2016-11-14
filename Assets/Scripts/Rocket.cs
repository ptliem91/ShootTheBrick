using UnityEngine;
using System.Collections;

public class Rocket : MonoBehaviour
{

	private Rigidbody2D myBody;

	private float speed = 5f;

	private float forceX, forceY;

	private bool isTop = false;

	// Use this for initialization
	void Awake ()
	{
		myBody = GetComponent<Rigidbody2D> ();
	}
	 
	// Update is called once per frame
	void FixedUpdate ()
	{
		myBody.velocity = new Vector2 (0, speed);

//		print (speed * Mathf.Cos (45f) + "-y: " + speed * Mathf.Sin (45f));
//		Vector3 v = new Vector3 ((speed * Mathf.Cos (45f)), (speed * Mathf.Sin (45f)), 0);
//		transform.position += v;

//		var shootDir = Quaternion.Euler (0, 0, 45f) * Vector3.right;
//		myBody.velocity = shootDir * speed; 

	}

	void OnTriggerEnter2D (Collider2D target)
	{
		if (target.tag == "Top") {
			Destroy (gameObject);
		}

		string[] name = target.name.Split ();

		if (name.Length > 1 && name [1] == "Ball") {
			Destroy (gameObject);
		}
	}
}
