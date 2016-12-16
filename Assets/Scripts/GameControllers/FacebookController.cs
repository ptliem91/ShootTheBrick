using UnityEngine;
using System.Collections;
using Facebook.Unity;
using UnityEngine.UI;
using System.Collections.Generic;

public class FacebookController : MonoBehaviour
{
	public static FacebookController instance;

	private const string PACKAGE_NAME = "com.LStudioGames.CutStoneHead";

	private void Awake ()
	{
		MakeSingleton ();

		if (!FB.IsInitialized) {
			FB.Init (InitCallback, OnHideUnity);
		
		} else {
			FB.ActivateApp ();
		}
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

	private void InitCallback ()
	{
		if (FB.IsInitialized) {
			// Signal an app activation App Event
			FB.ActivateApp ();
			// Continue with Facebook SDK
			// ...
		} else {
			Debug.Log ("Failed to Initialize the Facebook SDK");
		}
	}

	private void OnHideUnity (bool isGameShown)
	{
		if (!isGameShown) {
			// Pause the game - we will need to hide
			Time.timeScale = 0;
		} else {
			// Resume the game - we're getting focus again
			Time.timeScale = 1;
		}
	}

	public void LogIn ()
	{
		List<string> perms = new List<string> () { "public_profile, email", "user_friends" };
		FB.LogInWithReadPermissions (perms, OnLogIn);

//		FB.LogInWithPublishPermissions (new List<string> () { "publish_actions" });
	}

	private void OnLogIn (ILoginResult result)
	{
		if (FB.IsLoggedIn) {
			AccessToken token = AccessToken.CurrentAccessToken;
//			txtUserId.text = token.UserId;
			Debug.Log (token.UserId);

			// Print current access token's granted permissions
			foreach (string perm in token.Permissions) {
				Debug.Log (perm);
			}

		} else {
			Debug.Log ("User cancelled login");
		}
	}

	public void Share ()
	{
		FB.ShareLink (
			contentTitle: "Cut Stone Head",
			contentURL: new System.Uri ("market://details?id=" + PACKAGE_NAME),
			contentDescription: "A funny game for entertainment and relaxation! Join now!",
//			photoURL: new System.Uri ("https://www.google.com.vn/images/branding/googlelogo/2x/googlelogo_color_120x44dp.png"),
			callback: ShareCallback);

//		FB.FeedShare (link: new System.Uri ("http://www.google.ro"),
//			linkName: "GOOGLE", 
//			linkCaption: "The most popular search engine",
//			picture: new System.Uri ("http://www.zilesinopti.ro/media/6040188556717c5d366f86.95994291.jpg"),
//			callback: OnShare);
	}

	private void ShareCallback (IShareResult result)
	{
		if (result.Cancelled || !string.IsNullOrEmpty (result.Error)) {
//			Debug.Log ("Share error: " + result.Error);
			ShopMenuController.instance.PostFacebookFailed ();

		} else if (!string.IsNullOrEmpty (result.PostId)) {
//			Debug.Log ("result.PostId: " + result.PostId);

			// Print post identifier of the shared content
			Debug.Log (result.PostId);
			ShopMenuController.instance.PostFacebookFailed ();
		
		} else {
//			Debug.Log ("Share success!");
			ShopMenuController.instance.PostFacebookSucceded ();
		}
	}
}
