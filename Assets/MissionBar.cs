using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionBar : MonoBehaviour {

	public GameObject panel;
	public ProgressBar progressBar;
	public float totalHits;
	public float value;
	public Text field;
	public int sec;
	public Transform itemContainer;

	void Start () {
		panel.SetActive (false);
		Data.Instance.events.OnBossInit += OnBossInit;
		Data.Instance.events.OnBossActive += OnBossActive;
		Data.Instance.events.OnBossHitsUpdate += OnBossHitsUpdate;
		Data.Instance.events.OnBossSetNewAsset += OnBossSetNewAsset;
		Data.Instance.events.OnBossSetTimer += OnBossSetTimer;
		Data.Instance.events.OnGameOver += OnGameOver;
	}
	void OnDestroy () {
		Data.Instance.events.OnBossInit -= OnBossInit;
		Data.Instance.events.OnBossActive -= OnBossActive;
		Data.Instance.events.OnBossHitsUpdate -= OnBossHitsUpdate;
		Data.Instance.events.OnBossSetNewAsset -= OnBossSetNewAsset;
		Data.Instance.events.OnBossSetTimer -= OnBossSetTimer;
		Data.Instance.events.OnGameOver -= OnGameOver;
	}
	void OnGameOver(bool isTimeOut)
	{
		if (isTimeOut)
			return;
		panel.SetActive (false);
		CancelInvoke ();
	}
	void OnBossSetNewAsset(string assetName)
	{
		Utils.RemoveAllChildsIn (itemContainer);
		GameObject icon = Instantiate(Resources.Load("bosses/" + assetName, typeof(GameObject))) as GameObject;
		icon.transform.SetParent (itemContainer);
		icon.transform.localScale = Vector3.one;
		icon.transform.localPosition = Vector3.zero;
	}
	void OnBossHitsUpdate(float actualHits)
	{
		progressBar.SetProgression (1-(actualHits / totalHits));
	}
	void Loop()
	{		
		field.text = sec.ToString ();
		sec--;
		if (sec <= 9) {
			field.text = "0" + sec.ToString ();
			field.color = Color.red;
		} 
		if (sec <=  0) {
			Data.Instance.events.OnGameOver (true);
			Data.Instance.events.FreezeCharacters (true);
		} else {
			Invoke ("Loop", 1);
		}

	}
	void OnBossInit (int totalHits) {
		progressBar.SetProgression (1);
		this.totalHits = (float)totalHits;
		panel.SetActive (true);
	}
	void OnBossSetTimer(int timer)
	{
		if (timer == 0)
			timer = 50;
		
		if (Game.Instance.level.charactersManager.getTotalCharacters () == 1) {
			timer += 10;
		}
		
		if (timer > 60)
			timer = 60;	

		sec = timer;

		field.color = Color.white;

		Loop ();

	}
	void OnBossActive (bool isOn)
	{
		if (!isOn) {
			panel.SetActive (false);
			CancelInvoke ();
		}
	}
}
