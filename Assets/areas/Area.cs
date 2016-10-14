using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Area : MonoBehaviour {

	public float z_length;
    public int nextAreaX = 0;
	
	public GameObject[] getSceneObjects()
    {
        Level level = Game.Instance.level;
        List<GameObject> gos = new List<GameObject>();
        Transform[] childs = GetComponentsInChildren<Transform>(true);
        foreach (var t in childs)
        {
            if (t != transform)
            {
                if (t.tag == "sceneObject")
                {
                    gos.Add(t.gameObject);
                }
                else if (t.tag == "sceneObject_easy" && level.Dificulty == Level.Dificult.EASY)
                {
                    gos.Add(t.gameObject);
                }
                else if (t.tag == "sceneObject_medium" && level.Dificulty == Level.Dificult.MEDIUM)
                {
                    gos.Add(t.gameObject);
                }
                else if (t.tag == "sceneObject_hard" && level.Dificulty == Level.Dificult.HARD)
                {
                    gos.Add(t.gameObject);
                } 
            }
        }
        return gos.ToArray();
    }
}
