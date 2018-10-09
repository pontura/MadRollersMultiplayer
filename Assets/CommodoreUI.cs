using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommodoreUI : MonoBehaviour {

	public Text field;
//	public string[] texts;

	public Sprite[] spritesToBG;
	public GameObject bg;
	public Image[] backgroundImages;
	float speed;
	bool isOn;

	void Start () {
		ChangeBG ();
	}
	public void SetOn(bool _isOn)
	{
		this.isOn = _isOn;
		if (isOn)
			StartCoroutine (LoadingRoutine ());
	}
	IEnumerator LoadingRoutine()
	{
		Data.Instance.events.OnSoundFX("loading", 0);
		field.text = "";		
		AddText("*** MAD ROLLERS ***");
		yield return new WaitForSeconds (0.8f);
		AddText("Hacking " + Data.Instance.videogamesData.GetActualVideogameData ().name + " -> scores.list");
		yield return new WaitForSeconds (0.5f);
		AddText("Commander64 P_HASH[ASDL??89348");
		yield return new WaitForSeconds (0.8f);
		AddText("Write Permisson Accepted!");
		yield return new WaitForSeconds (0.5f);
		SetOn (false);
		//Data.Instance.events.OnGameStart();
		yield return null;
	}
	void AddText(string text)
	{
		field.text += text +'\n';
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
