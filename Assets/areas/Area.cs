using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Area : MonoBehaviour {

	public float z_length;
    public int nextAreaX = 0;
	
	public GameObject[] getSceneObjects()
    {
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
            }
        }
        return gos.ToArray();
    }
}
