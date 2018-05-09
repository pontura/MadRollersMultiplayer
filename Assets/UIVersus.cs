using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIVersus : MonoBehaviour {

	public GameObject gameOverPanel;
	public GameObject countDownPanel;
	public GameObject summary;
	public Text summaryField;

	int score1;
	int score2;

	public Text score1Field;
	public Text score2Field;

	public Text countDownField;
	int countDown = 3;

	public states state;
	public enum states
	{
		PLAYING,
		READY
	}

	void Start () {
		Data.Instance.events.OnGameOver += OnGameOver;
		Data.Instance.events.OnScoreOn += OnScoreOn;
		gameOverPanel.SetActive (false);
		summary.SetActive (false);
		countDownPanel.SetActive (true);
		CountDown ();
		SetScores ();
	}
	void OnDestroy () {
		Data.Instance.events.OnGameOver -= OnGameOver;
		Data.Instance.events.OnScoreOn -= OnScoreOn;
	}
	void OnScoreOn(int playerID, Vector3 pos, int qty)
	{
		if (playerID == 0)
			score1++;
		else
			score2++;
		SetScores ();
	}
	void SetScores()
	{
		score1Field.text = score1.ToString ();
		score2Field.text = score2.ToString ();
	}
	void OnGameOver()
	{
		if (state == states.READY)
			return;
		
		state = states.READY;
		summary.SetActive (true);
		summaryField.text = "DONE!";
		StartCoroutine (Restart ());
	}
	IEnumerator Restart ()
	{
		yield return new WaitForSeconds (4);
		Data.Instance.events.OnResetLevel ();
		yield return new WaitForSeconds (0.2f);
		Data.Instance.LoadLevel ("MainMenuArcade");
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
