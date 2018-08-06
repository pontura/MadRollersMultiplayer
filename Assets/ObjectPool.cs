using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[AddComponentMenu("Gameplay/ObjectPool")]
public class ObjectPool : MonoBehaviour
{

    #region member
    [Serializable]
    public class ObjectPoolEntry
    {
        [SerializeField]
        public SceneObject Prefab;

        [SerializeField]
        public int Count;
    }
    #endregion
    public ObjectPoolEntry[] Entries;
    [HideInInspector]
    public GameObject Scene;

    public static ObjectPool instance;
    public List<SceneObject> pooledObjects;
    protected GameObject containerObject;


    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        DontDestroyOnLoad(this);

        containerObject = new GameObject("ObjectPool");
        Scene = new GameObject("Scene");

        DontDestroyOnLoad(containerObject);
        DontDestroyOnLoad(Scene);
        
//		foreach (ObjectPoolEntry poe in Entries) {
//			for (int a=0; a<poe.Count; a++) {
//				pooledObjects.Add (poe.Prefab);
//			}
//		}


        int id = 0;
        for (int i = 0; i < Entries.Length; i++)
        {
            var objectPrefab = Entries[i];
            //create the repository


            for (int n = 0; n < objectPrefab.Count; n++)
            {
				SceneObject newObj = CreateSceneObject (objectPrefab.Prefab);
				print (newObj.name);
//                SceneObject newObj = Instantiate(objectPrefab.Prefab) as SceneObject;
//				pooledObjects.Add (newObj);
//                newObj.name = objectPrefab.Prefab.name;
				PoolObject(newObj);
                newObj.id = id;
                id++;

            }
        }
        Restart();
        
    }
	SceneObject CreateSceneObject(SceneObject so)
	{
		SceneObject newSO = Instantiate(so) as SceneObject;
		newSO.name = so.name;
		return newSO;
	}
    void Restart()
    {

    }
    private int GetTotalObjectsOfType(string objectType)
    {
        int qty = 0;
        foreach (SceneObject so in containerObject.GetComponentsInChildren<SceneObject>())
        {
            if (so.name == objectType)
            {
                qty++;
            }
        }
        return qty;
    }


    public SceneObject GetObjectForType(string objectType, bool onlyPooled)
    {

		SceneObject pooledObject = null;
		foreach (SceneObject soPooled in pooledObjects) {
			if (soPooled.name == objectType || soPooled.name + "(Clone)" == objectType) {
				pooledObject = soPooled;
			}
		}
				
        if (pooledObject != null)
        {
			pooledObject.transform.SetParent( Scene.transform );
			pooledObjects.Remove(pooledObject);	
            return pooledObject;
		} 
		if (!onlyPooled)
		{
			foreach (ObjectPoolEntry poe in Entries) {
				if (poe.Prefab.name == objectType || poe.Prefab.name + "(Clone)" == objectType) {	
					SceneObject newSceneObject = CreateSceneObject(poe.Prefab);
					newSceneObject.transform.parent = Scene.transform;
					pooledObjects.Remove(newSceneObject);	
					return newSceneObject;
				}
			}
		} 
	//	Debug.Log("_________________NO HAY: " + objectType + "  bool " + onlyPooled);
		return null;
    }




    public void PoolObject(SceneObject obj)
    {
		if (obj.broken) {
			//print ("____________broken " + obj.name);
			Destroy (obj.gameObject);
			return;
		}
        for (int i = 0; i < Entries.Length; i++)
        {
            if (Entries[i].Prefab.name == obj.name || Entries[i].Prefab.name + "(Clone)" == obj.name)
            {        				
				obj.transform.SetParent(containerObject.transform);
				obj.gameObject.SetActive(false);
                pooledObjects.Add(obj);
                return;
            }
        }
        Destroy(obj.gameObject);
    }



}