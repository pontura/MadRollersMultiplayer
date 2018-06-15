using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour {
	
	void Start () {
		Invoke("Next", 1);
	}
	void Next () {
		//if(Data.Instance.isArcadeMultiplayer)
		//	Data.Instance.LoadLevel("01LandingPageForArcade");
	//	else
		//	Data.Instance.LoadLevel("01LandingPage");
		Data.Instance.LoadLevel("MainMenu");
	}
}
