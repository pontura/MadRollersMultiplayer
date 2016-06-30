using UnityEngine;
using System.Collections;

public class LandingForArcade : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Invoke("Next", 1);
        //Cursor.visible = false;
	}
    void Next()
    {
        Data.Instance.playMode = Data.PlayModes.COMPETITION;
        if (Data.Instance.isArcadeMultiplayer)
            Data.Instance.LoadLevel("MainMenuArcade");
        else
            Data.Instance.LoadLevel("GameForArcade");
    }
}
