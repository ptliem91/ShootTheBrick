using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class PlayerMovementScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

	public void OnPointerDown (PointerEventData data)
	{
		print ("OnPointerDown");

		if (this.gameObject.tag == "MoveLeftButton") {
			Player.instance.MovePlayerLeft ();
		} else if (this.gameObject.tag == "MoveRightButton") {
			Player.instance.MovePlayerRight ();
		}
	}

	public void OnPointerUp (PointerEventData data)
	{
		print ("OnPointerUp");

		Player.instance.StopMoving ();
	}
}
