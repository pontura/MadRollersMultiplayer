using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class SummaryVersus : MonoBehaviour {

	public GameObject panel;
	private int countDown;

	public List<MainMenuButton> buttons;
	public int optionSelected = 0;
	public bool isOn;

	void Start()
	{
		panel.SetActive(false);
	}
	public void SetOn()
	{
		Invoke ("Delayed", 2);
	}
	public void Delayed()
	{
		Data.Instance.events.RalentaTo (1, 0.05f);
		isOn = true;
		panel.SetActive(true);
		SetSelected ();
	}
	void Update()
	{
		if (!isOn)
			return;

		lastClickedTime += Time.deltaTime;
		if (lastClickedTime > 0.2f)
			processAxis = true;
		for (int a = 0; a < 4; a++) {
			if (InputManager.getJump (a)) 
				OnJoystickClick ();
			if (InputManager.getFire (a)) 
				OnJoystickClick ();
			if (processAxis) {
				float v = InputManager.getHorizontal (a) + InputManager.getVertical (a);
				if (v < -0.5f) {
					lastClickedTime = 0;
					processAxis = false;
					OnJoystickDown ();
				} else if (v > 0.5f) {
					OnJoystickUp ();
					lastClickedTime = 0;
					processAxis = false;
				}
			}
		}
	}


	float lastClickedTime = 0;
	bool processAxis;

	void OnJoystickUp () {
		if (optionSelected >= buttons.Count - 1)
			return;
		optionSelected++;
		SetSelected ();
	}
	void OnJoystickDown () {
		if (optionSelected <= 0)
			return;
		optionSelected--;
		SetSelected ();
	}
	void SetSelected()
	{
		foreach(MainMenuButton b in buttons)
			b.SetOn (false);
		buttons [optionSelected].SetOn (true);
	}

	void OnJoystickClick () {
		
		Data.Instance.events.ForceFrameRate (1);
		UIVersus uiversus = GetComponent<UIVersus> ();
		if (optionSelected == 0) {
			StartCoroutine(uiversus.Reset (1));
		} else if (optionSelected == 1) {			
			StartCoroutine(uiversus.Reset (2));
		} else if (optionSelected == 2) {
			StartCoroutine(uiversus.Reset (3));
		}
		isOn = false;
	}
	void OnJoystickBack () {
		//Data.Instance.events.OnJoystickBack ();
	}
	void ResetMove()
	{
		processAxis = false;
		lastClickedTime = 0;
	}
}
