﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICountDown : MonoBehaviour {

	public GameObject panel;

	public Text countDownField;
	int countDown = 3;

	void Start () {
		
		if (Data.Instance.playMode == Data.PlayModes.STORY || Data.Instance.isReplay) {
			panel.SetActive (false);
			return;
		}
		Data.Instance.events.OnGameStart += OnGameStart;		
		panel.SetActive (true);
	}
	void OnDestroy()
	{
		Data.Instance.events.OnGameStart -= OnGameStart;
	}
	void OnGameStart()
	{
		countDownField.text = countDown.ToString ();
		Invoke ("SetNextCountDown", 1.2f);
	}
	void SetNextCountDown()
	{
		panel.GetComponent<Animation>().Play("logo");
		countDown--;
		if (countDown == 0) {
			Data.Instance.events.StartMultiplayerRace ();
			panel.SetActive (false);
			return;
		}
		OnGameStart ();
	}
}
