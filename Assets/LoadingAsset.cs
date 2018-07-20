using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingAsset : MonoBehaviour {

	public GameObject loadingPanel;
	public GameObject videoPanel;

	public GameObject panel;
	public Sprite[] spritesToBG;
	public GameObject bg;
	public Image[] backgroundImages;
	float speed;
	bool isOn;
	void Start () {
		videoPanel.SetActive (false);
		ChangeBG ();
	}
	public void SetOn(bool _isOn)
	{
		videoPanel.SetActive (true);
		this.isOn = _isOn;
		//panel.SetActive (_isOn);
		loadingPanel.SetActive (_isOn);
		Data.Instance.events.StartMultiplayerRace += StartMultiplayerRace;
		Data.Instance.events.OnGameOver += OnGameOver;
	}
	void StartMultiplayerRace()
	{
		videoPanel.SetActive (false);
	}
	void OnGameOver()
	{
		videoPanel.SetActive (true);
	}


	void Update () {
		
		if (!isOn)
			return;
		
		Vector2 pos = bg.transform.localPosition;
		pos.y += speed * Time.deltaTime;
		if (pos.y > 0) {
			ChangeBG ();
			pos.y = -90;
		}
		bg.transform.localPosition = pos;
		Invoke ("ChangeSpeed", (float)Random.Range (5, 30) / 10);
	}
	void ChangeSpeed()
	{
		speed = (float)Random.Range (100, 400);
	}
	void ChangeBG()
	{
		Sprite s = spritesToBG [Random.Range (0, spritesToBG.Length)];
		foreach (Image image in backgroundImages)
			image.sprite = s;
	}
}
