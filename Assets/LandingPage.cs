using UnityEngine;
using System.Collections;

public class LandingPage : MonoBehaviour {


	void Start () {
		Data.Instance.events.OnJoystickClick += OnJoystickClick;
	}
	void OnDestroy () {
		Data.Instance.events.OnJoystickClick -= OnJoystickClick;
	}
	void OnJoystickClick () {
		Data.Instance.LoadLevel("MainMenu");
	}
}
