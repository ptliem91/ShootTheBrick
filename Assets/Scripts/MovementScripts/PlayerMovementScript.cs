using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class PlayerMovementScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

	public void OnPointerDown (PointerEventData data)
	{
		Player.instance.StopAnimationAppear ();

		if (this.gameObject.tag == "MoveLeftButton") {
			Player.instance.MovePlayerLeft ();
		} else if (this.gameObject.tag == "MoveRightButton") {
			Player.instance.MovePlayerRight ();
		}
	}

	public void OnPointerUp (PointerEventData data)
	{
		Player.instance.StopAnimationAppear ();

		Player.instance.StopMoving ();
	}
}
