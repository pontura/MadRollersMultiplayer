using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBarMultiplayer : MonoBehaviour {

	public GameObject hiscoreFields;
	public GameObject myScoreFields;
	public Image bar;
	public RawImage hiscoreImage;
	public int score;
	public int hiscore;
	bool hiscoreWinned;

	void Start () {
		hiscoreWinned = false;

		Data.Instance.events.OnScoreOn += OnScoreOn;

		score = 0;
		bar.fillAmount = 0;

		ArcadeRanking arcadeRanking = Data.Instance.GetComponent<ArcadeRanking> ();
		if (arcadeRanking.all.Count > 0) {
			hiscore = arcadeRanking.all [0].score;
			foreach (Text textfield in hiscoreFields.GetComponentsInChildren<Text>())
				textfield.text = hiscore.ToString ();

			hiscoreImage.material.mainTexture = Data.Instance.GetComponent<ArcadeRanking>().all[0].texture;
		}
		UpdateScore ();
	}
	void OnDestroy()
	{
		Data.Instance.events.OnScoreOn -= OnScoreOn;
	}
	void OnScoreOn(int playerID, Vector3 pos, int total)
	{
		score += total;
		UpdateScore ();
	}
	void UpdateScore()
	{	

		foreach (Text textfield in myScoreFields.GetComponentsInChildren<Text>())
			textfield.text = score.ToString ();

		if(hiscoreWinned) return;
		
		float barValue = (float)score/(float)hiscore;
		if (barValue >= 1) {
			hiscoreWinned = true;
			barValue = 1;
			Data.Instance.events.ShowNotification ("NUEVO HISCORE!");
		}
		bar.fillAmount = barValue;
	}
}
