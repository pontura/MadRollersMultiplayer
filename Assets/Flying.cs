using UnityEngine;
using System.Collections;

public class Flying : SceneObject {

    public override void OnRestart(Vector3 pos)
    {
        base.OnRestart(pos);
        GetComponent<Breakable>().OnBreak += OnBreak;

    }
    public override void OnPool()
    {
        base.OnPool();
        GetComponent<Breakable>().OnBreak -= OnBreak;
    }
    private void OnBreak()
    {
        setScore();
        Missions missions = Data.Instance.GetComponent<Missions>();
        missions.killPlane();
        isActive = false;
    }
}
