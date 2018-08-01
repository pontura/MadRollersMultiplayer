using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBarMultiplayer : MonoBehaviour {

	int ScoreToWinCredit;
	int NextScoreToWinCredit;
	public GameObject panel;
	public GameObject hiscoreFields;
	public Text myScoreFields;
	public Image bar;
	public RawImage hiscoreImage;
	public int hiscore;
    float newVictoryAreaScore;

    bool hiscoreWinned;
    

    void Start () {
		ScoreToWinCredit = Data.Instance.multiplayerData.ScoreToWinCredit;
		SetNextScoreToWinCredit ();
//		if (Data.Instance.playMode == Data.PlayModes.STORY) {
//			panel.SetActive (false);
//			return;
//		} else {
//			panel.SetActive (true);
//		}
	//	hiscoreWinned = false;
      //  newVictoryAreaScore = Data.Instance.multiplayerData.newVictoryAreaScore;

        Data.Instance.events.OnScoreOn += OnScoreOn;

//		bar.fillAmount = 0;
//
//		ArcadeRanking arcadeRanking = Data.Instance.GetComponent<ArcadeRanking> ();
//		if (arcadeRanking.all.Count > 0) {
//			hiscore = arcadeRanking.all [0].score;
//			foreach (Text textfield in hiscoreFields.GetComponentsInChildren<Text>())
//				textfield.text = hiscore.ToString ();
//
//			hiscoreImage.material.mainTexture = Data.Instance.GetComponent<ArcadeRanking>().all[0].texture;
//		}
		UpdateScore ();
	}
	void SetNextScoreToWinCredit()
	{
		NextScoreToWinCredit = (Data.Instance.multiplayerData.creditsWon+1)*ScoreToWinCredit;
		print ("NextScoreToWinCredit " + NextScoreToWinCredit);
	}
	void OnDestroy()
	{
		Data.Instance.events.OnScoreOn -= OnScoreOn;
	}
	void OnScoreOn(int playerID, Vector3 pos, int total)
	{
		if (NextScoreToWinCredit < Data.Instance.multiplayerData.score) {
			Data.Instance.multiplayerData.creditsWon ++;
			SetNextScoreToWinCredit ();
			GetComponent<CreditsUI> ().AddNewCredit ();
		}
		Data.Instance.multiplayerData.score += total;
		UpdateScore ();
//		if(Data.Instance.multiplayerData.score > newVictoryAreaScore)
//        {
//			print ("__newVictoryAreaScore " + newVictoryAreaScore + "  newVictoryAreaScore" + Data.Instance.multiplayerData.newVictoryAreaScore);
//            Data.Instance.events.SetVictoryArea();
//			Data.Instance.multiplayerData.newVictoryAreaScore *= 1.5f;
//            this.newVictoryAreaScore += Data.Instance.multiplayerData.newVictoryAreaScore;
//        }
	}
	void UpdateScore()
	{	
		if (Data.Instance.multiplayerData.score == 0)
			myScoreFields.text = "000";
		else
			myScoreFields.text = string.Format ("{0:#,#}",  Data.Instance.multiplayerData.score);



//		return;
//
//		if(hiscoreWinned) return;
//		
//		float barValue = (float)score/(float)hiscore;
//		if (barValue >= 1) {
//			hiscoreWinned = true;
//			barValue = 1;
//			Data.Instance.events.ShowNotification ("NUEVO HISCORE!");
//		}
//		bar.fillAmount = barValue;
	}
}
