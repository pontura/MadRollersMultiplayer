using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Area : MonoBehaviour {

	public float z_length;
    public int nextAreaX = 0;
	public List<GameObject> gos;
    public int totalCoins;

	public List<GameObject> getSceneObjects()
    {
		
		if (gos.Count > 0 && totalCoins>0) {
			return gos;
		}
		print("First time level loaded: " + name);
		
        gos = new List<GameObject>();
        Transform[] childs = GetComponentsInChildren<Transform>(true);
        foreach (var t in childs)
        {
           // if (t != transform)
           // {
               
                if (t.tag == "sceneObject")
                {
                    gos.Add(t.gameObject);
					t.gameObject.SetActive (false);

                    if(t.name=="Coin")
                        totalCoins++;
                }
//                else if (t.tag == "sceneObject_easy" && level.Dificulty == Level.Dificult.EASY)
//                {
//                    gos.Add(t.gameObject);
//                }
//                else if (t.tag == "sceneObject_medium" && level.Dificulty == Level.Dificult.MEDIUM)
//                {
//                    gos.Add(t.gameObject);
//                }
//                else if (t.tag == "sceneObject_hard" && level.Dificulty == Level.Dificult.HARD)
//                {
//                    gos.Add(t.gameObject);
//                } 
          //  }
        }
		return gos;
    }
}
