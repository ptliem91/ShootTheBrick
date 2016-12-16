using UnityEngine;
using System.Collections;

public class InternetChecker : MonoBehaviour
{
	public static InternetChecker instance;

	private const bool allowCarrierDataNetwork = false;
	private const string pingAddress = "8.8.8.8";
	// Google Public DNS server
	private const float waitingTime = 2.0f;

	private Ping ping;
	private float pingStartTime;

	public bool isConnected = false;

	void Awake ()
	{
		MakeSingleton ();
	}

	public void Start ()
	{
		bool internetPossiblyAvailable;
		switch (Application.internetReachability) {
		case NetworkReachability.ReachableViaLocalAreaNetwork:
			internetPossiblyAvailable = true;
			break;
		case NetworkReachability.ReachableViaCarrierDataNetwork:
			internetPossiblyAvailable = allowCarrierDataNetwork;
			break;
		default:
			internetPossiblyAvailable = false;
			break;
		}
		if (!internetPossiblyAvailable) {
			isConnected = false;
			return;
		}
		ping = new Ping (pingAddress);
		pingStartTime = Time.time;
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

	public void Update ()
	{
		if (ping != null) {
			bool stopCheck = true;
			if (ping.isDone) {
				if (ping.time >= 0) {
					isConnected = true;
				} else {
					isConnected = false;
				}
	
			} else if (Time.time - pingStartTime < waitingTime) {
				stopCheck = false;

			} else {
				isConnected = false;

			}

			if (stopCheck) {
				ping = null;
			}
		}
	}
}
