using UnityEngine;
using System.Collections;

public static class MyCoroutine
{

	public static IEnumerator WaitForRealSeconds (float waitTime)
	{
		float start = Time.realtimeSinceStartup;

		while (Time.realtimeSinceStartup < (start + waitTime)) {
			yield return null;
		}
	}
}
