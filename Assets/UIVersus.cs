using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIVersus : MonoBehaviour {

	public GameObject gameOverPanel;

	int score1;
	int score2;

	public Text score1Field;
	public Text score2Field;

	public states state;
	public enum states
	{
		PLAYING,
		READY
	}

	void Start () {
		Data.Instance.events.OnScoreOn += OnScoreOn;
		gameOverPanel.SetActive (false);
		SetScores ();
	}
	void OnDestroy () {
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
	public void OnGameOver()
	{
		if (state == states.READY)
			return;		
		Data.Instance.events.ForceFrameRate (1);
		state = states.READY;
		StartCoroutine (Restart ());
	}
	IEnumerator Restart ()
	{
		yield return new WaitForSeconds (3);
		Data.Instance.GetComponent<Fade> ().FadeToBlack ();
		yield return new WaitForSeconds (1);
		Data.Instance.events.OnResetLevel ();
		yield return new WaitForSeconds (0.2f);
		Data.Instance.LoadLevelNotFading ("GameVersus");
	}
}
