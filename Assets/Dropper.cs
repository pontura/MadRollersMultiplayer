using UnityEngine;
using System.Collections;

public class Dropper : MonoBehaviour
{
	public string SceneObjectName;
    public float delay = 1;
    public float delayRandom = 0;

    float sec;

    void Start()
    {
        sec = 0;
		SetRandom ();
    }
    public void SetRandom()
    {
		delayRandom = Random.Range(delay, delay+delayRandom);
    }

    void Update()
    {
        if (sec > delayRandom)
        {
			SetRandom ();
            Vector3 pos = transform.position;
			SceneObject newSceneObject;
			newSceneObject = Data.Instance.sceneObjectsPool.GetObjectForType(SceneObjectName, false);  
			Game.Instance.sceneObjectsManager.AddSceneObjectAndInitIt(newSceneObject, pos);         
            sec = 0;
        }
        sec += Time.deltaTime;
    }
}
