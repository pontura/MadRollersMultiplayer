using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickController : MonoBehaviour {

	float lastClickedTime = 0;
	bool processAxis;

	void Update()
	{
		lastClickedTime += Time.deltaTime;
		if (lastClickedTime > 0.1f)
			processAxis = true;
		for (int a = 0; a < 4; a++) {
			if (InputManager.getJump (a)) 
				OnJoystickClick ();
			if (InputManager.getFire (a)) 
				OnJoystickClick ();
			if (processAxis) {
				float v = InputManager.getVertical (a);
				if (v < -0.5f)
					OnJoystickUp ();
				else if (v > 0.5f)
					OnJoystickDown ();
				float h = InputManager.getHorizontal (a);

				if (h < -0.5f)
					OnJoystickRight ();
				else if (h > 0.5f)
					OnJoystickLeft ();
			}
		}
	}
	void OnJoystickUp () {
		Data.Instance.events.OnJoystickUp ();
		ResetMove ();
	}
	void OnJoystickDown () {
		Data.Instance.events.OnJoystickDown ();
		ResetMove ();
	}
	void OnJoystickRight () {
		Data.Instance.events.OnJoystickRight ();
		ResetMove ();
	}
	void OnJoystickLeft () {
		Data.Instance.events.OnJoystickLeft ();
		ResetMove ();
	}
	void OnJoystickClick () {
		Data.Instance.events.OnJoystickClick ();
	}
	void OnJoystickBack () {
		Data.Instance.events.OnJoystickBack ();
	}
	void ResetMove()
	{
		processAxis = false;
		lastClickedTime = 0;
	}
}
