using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICountDown : MonoBehaviour {

	public GameObject panel;

	public Text countDownField;
	int countDown = 3;

	void Start () {
		if (Data.Instance.playMode == Data.PlayModes.STORY) {
			panel.SetActive (false);
			return;
		}
		
		panel.SetActive (true);
		CountDown ();
	}
	void CountDown()
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
		CountDown ();
	}
}
