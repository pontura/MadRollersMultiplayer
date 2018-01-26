using UnityEngine;
using System.Collections;

public class LandingPage : MonoBehaviour {

    public GameMenu gm;

	void Start () {
       // if (gm && Input.anyKeyDown)
      //  {
          //  gm.SetOn();
		Invoke("DoIt", 3);
       //}
	}
	void DoIt () {
		Data.Instance.LoadLevel("MainMenu");
	}
}
