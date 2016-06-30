using UnityEngine;
using System.Collections;

public class TrainingReady : MonoBehaviour {

    public void Continue()
    {
        Data.Instance.playingTutorial = false;
        Data.Instance.playMode = Data.PlayModes.STORY;
        Data.Instance.LoadLevel("Game");
    }
    public void Compite()
    {
        Data.Instance.playingTutorial = false;
        Data.Instance.playMode = Data.PlayModes.COMPETITION;
        Data.Instance.LoadLevel("Competitions");
    }
}
