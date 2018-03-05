using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioWriter : MonoBehaviour {

	public Text field;
	public float speed = 0.1f;

	int sentenceID = 0;
	int letterId = 0;
	int totalWords;
	string sentence;

	void Start () {
		Next ();
	}
	void Next()
	{
		if (sentenceID == Data.Instance.voicesManager.intros.Count) {
			Data.Instance.LoadLevel("MainMenu");
			return;
		}
		SetText(Data.Instance.voicesManager.intros[sentenceID].text);
		Data.Instance.voicesManager.PlayClip (Data.Instance.voicesManager.intros[sentenceID].audioClip);
		sentenceID++;
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
			field.text = field.text.Remove (field.text.Length-1,1);
			ChangeSentence ();
			return;
		}
		field.text = field.text.Remove (field.text.Length-1,1);
		field.text += sentence [letterId] + "_";
		letterId++;
		Invoke ("WriteLoop", speed);
	}
	void ChangeSentence()
	{
		Invoke ("Next", 3);
	}

}
