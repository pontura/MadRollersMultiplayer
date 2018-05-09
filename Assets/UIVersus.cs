using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIVersus : MonoBehaviour {

	public GameObject gameOverPanel;
	public GameObject countDownPanel;
	public Text countDownField;
	int countDown = 3;

	void Start () {
		gameOverPanel.SetActive (false);
		countDownPanel.SetActive (true);
		CountDown ();
	}
	void CountDown()
	{
		countDownField.text = countDown.ToString ();
		Invoke ("SetNextCountDown", 1);
	}
	void SetNextCountDown()
	{
		countDown--;
		if (countDown == 0) {
			Data.Instance.events.StartMultiplayerRace ();
			countDownPanel.SetActive (false);
			return;
		}
		CountDown ();
	}
}
