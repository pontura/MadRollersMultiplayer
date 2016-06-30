using UnityEngine;
using System.Collections;

public class TrainingSplash : MonoBehaviour {

	void Start () {
        Data.Instance.events.VoiceFromResources("ok_pero_primero_demuestra_que_puedes");
        Invoke("Go", 3);
        Data.Instance.playMode = Data.PlayModes.STORY;
        Data.Instance.playingTutorial = true;
	}
    void Go()
    {
        Data.Instance.LoadLevel("Game");
    }
}
