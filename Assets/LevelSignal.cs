using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelSignal : SceneObject
{
    public Text field;
 
    public override void OnRestart(Vector3 pos)
    {
        //field.text = "MISSION " + Data.Instance.missionActive.ToString();
        base.OnRestart(pos);
    }
}
