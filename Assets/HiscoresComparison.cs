using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HiscoresComparison : MonoBehaviour {

	public HiscoresComparisonSignal signal;
	public GameObject panel;
	public Transform container;
	ArcadeRanking arcadeRanking;
	public float hiscore;
	public float topTenPercent;
	public Image topTenImage;
	HiscoresComparisonSignal mySignal;
	int score;
	int puesto;

	void Start () {
		panel.SetActive (false);
		arcadeRanking = Data.Instance.GetComponent<ArcadeRanking> ();
	}

	public void Init() {
		if (arcadeRanking.all.Count == 0)
			return;

		hiscore = (float)arcadeRanking.all [0].hiscore;

		SetTopTenImage ();

		panel.SetActive (true);
		score = Data.Instance.multiplayerData.score;
		ArcadeRanking.Hiscore myTempHiscore = new ArcadeRanking.Hiscore ();
		myTempHiscore.hiscore = score;
		myTempHiscore.username = "TU SCORE";
		mySignal = AddSignal (myTempHiscore, 120);
		float gotoX = mySignal.transform.localPosition.x;
		Vector3 pos = mySignal.transform.localPosition;
		pos.x = 0;
		mySignal.transform.localPosition = pos;

		iTween.MoveTo(mySignal.gameObject, iTween.Hash(
			"x", gotoX,
			"islocal", true,
			"time", 2
		));
		StartCoroutine (DrawHiscores ());
	}
	IEnumerator DrawHiscores()
	{
		puesto = 0;
		int num = 1;
		foreach(ArcadeRanking.Hiscore data in arcadeRanking.all)
		{			
			if (num > 10) {
				SetPuesto ();
				yield return new WaitForSeconds (15f);
				Reset ();
			} else {
				if (puesto== 0 && data.hiscore < hiscore)
					puesto = num;
				yield return new WaitForSeconds (0.25f);
				AddSignal (data, num);
				num++;
			}
		}
	}
	void SetPuesto()
	{
		if (puesto == 0)
			Data.Instance.voicesManager.PlaySpecificClipFromList (Data.Instance.voicesManager.UIItems, 5);
		else
			Data.Instance.voicesManager.PlaySpecificClipFromList (Data.Instance.voicesManager.UIItems, 4);
		
		mySignal.SetPuesto (puesto);
	}
	HiscoresComparisonSignal AddSignal(ArcadeRanking.Hiscore data, int puesto)
	{
		float normalizedPos = GetNormalizedPosition (data.hiscore);
		HiscoresComparisonSignal newSignal = Instantiate (signal);
		newSignal.transform.SetParent (container);
		newSignal.transform.localScale = Vector3.one;

		if(puesto ==1 || puesto >10)
			newSignal.Init (data.username, data.hiscore, puesto);
		else
			newSignal.Init ("",0, 0);

		float _x = GetNormalizedPosition(data.hiscore);
		print (_x);
		newSignal.transform.localPosition = new Vector3 (_x ,0,0);
		return newSignal;
	}
	float GetNormalizedPosition(int score)
	{
		return ((float)score * 100) / hiscore;
	}
	public void Reset()
	{
		StopAllCoroutines ();
		panel.SetActive (false);
	}
	void SetTopTenImage()
	{
		int lastScore;
		if (arcadeRanking.all.Count >= 10)
			lastScore = arcadeRanking.all [9].hiscore;
		else
			lastScore = arcadeRanking.all [arcadeRanking.all.Count-1].hiscore;
		
		float f = GetNormalizedPosition (lastScore) / 100;
		topTenImage.fillAmount = f;
	}
}
