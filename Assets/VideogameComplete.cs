using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideogameComplete : MonoBehaviour {

	public GameObject enrollButton;

	public Text field;
	public float speed = 0.1f;

	int sentenceID = 0;
	int letterId = 0;
	int totalWords;
	string sentence;
	bool done;

	void Start () {
		Data.Instance.events.OnJoystickClick += OnJoystickClick;
		Data.Instance.events.OnJoystickBack += OnJoystickClick;

		enrollButton.SetActive (false);
		SetText("Congratulations! This videogame is completely ruined!");
	}
	void OnDestroy()
	{
		Data.Instance.events.OnJoystickClick -= OnJoystickClick;
		Data.Instance.events.OnJoystickBack -= OnJoystickClick;
	}
	void SetText (string sentence) {
		this.sentence = sentence;
		field.text = ">";
		letterId = 0;
		totalWords = sentence.Length;
		WriteLoop ();
	}
	void WriteLoop()
	{
		if (letterId == totalWords) {
			enrollButton.SetActive (true);
			return;
		}
		field.text = field.text.Remove (field.text.Length-1,1);
		field.text += sentence [letterId] + "_";
		letterId++;
		Invoke ("WriteLoop", speed);
	}
	public void OnJoystickClick()
	{
		if(Data.Instance.videogamesData.actualID == 0)
			Data.Instance.videogamesData.actualID = 1;
		else
			Data.Instance.videogamesData.actualID = 0;
		Data.Instance.LoadLevel ("LevelSelector");
	}
}