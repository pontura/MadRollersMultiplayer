using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class MainMenuButton : MonoBehaviour {
	
	public Text[] overs;
	public GameObject selected;

	public void SetOn(bool isOn)
	{
		if (isOn) {
			foreach (Text m in overs)
				m.color = Color.yellow;
			selected.SetActive (true);
		} else {
			foreach (Text m in overs)
			{
				m.color = Color.white;
			}
			selected.SetActive (false);
		}
	}
}
