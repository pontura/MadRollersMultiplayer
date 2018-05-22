using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIVersus : MonoBehaviour {

	public GameObject gameOverPanel;

	public GameObject bg_videogame1;
	public GameObject bg_videogame2;

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
		if (Data.Instance.videogamesData.actualID == 0) {
			bg_videogame1.SetActive (true);
			bg_videogame2.SetActive (false);
		} else {
			bg_videogame1.SetActive (false);
			bg_videogame2.SetActive (true);
		}
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
		//score1Field.text = score1.ToString ();
		//score2Field.text = score2.ToString ();
	}
	public void OnGameOver()
	{
		if (state == states.READY)
			return;		
		Data.Instance.events.ForceFrameRate (1);
		state = states.READY;
		GetComponent<SummaryVersus> ().SetOn ();
	}
	public IEnumerator Reset (int id)
	{
		//yield return new WaitForSeconds (3);
		Data.Instance.GetComponent<Fade> ().FadeToBlack ();
		yield return new WaitForSeconds (1);
		Data.Instance.events.OnResetLevel ();
		yield return new WaitForSeconds (0.2f);
		if (id == 1)
			Data.Instance.LoadLevelNotFading ("GameVersus");
		else if (id == 2) {			
			Data.Instance.LoadLevelNotFading ("LevelSelector");
		} else {			
			Data.Instance.playMode = Data.PlayModes.COMPETITION;
			Data.Instance.LoadLevelNotFading ("LevelSelector");
		}
	}
}
