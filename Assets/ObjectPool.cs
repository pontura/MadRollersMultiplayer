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
    public List<SceneObject>[] pooledObjects;
    protected GameObject containerObject;

    private int totalEntries;

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
        
        for (int i = 0; i < Entries.Length; i++)
        {
            totalEntries += Entries[i].Count;
        }

        pooledObjects = new List<SceneObject>[totalEntries];

        int id = 0;
        for (int i = 0; i < Entries.Length; i++)
        {
            var objectPrefab = Entries[i];
            //create the repository

            pooledObjects[i] = new List<SceneObject>();
            //fill it

            for (int n = 0; n < objectPrefab.Count; n++)
            {
                SceneObject newObj = Instantiate(objectPrefab.Prefab) as SceneObject;
                newObj.name = objectPrefab.Prefab.name;
                SceneObject ro = newObj.GetComponent<SceneObject>();
                PoolObject(ro);
                newObj.id = id;
                id++;

            }
        }
        Restart();
        
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
        for (int i = 0; i < Entries.Length; i++)
        {
            var prefab = Entries[i].Prefab;
            if (prefab.name != objectType)
                continue;

            if (pooledObjects[i].Count > 0)
            {
                SceneObject pooledObject = pooledObjects[i][0];
                pooledObjects[i].RemoveAt(0);
                if (pooledObject)
                {
					if (pooledObject.gameObject.name == "bloodx1_real" || pooledObject.gameObject.name == "bloodx1_real(Clone)") 
						print ("pone en SCene: bloodx1_real");
					
                    pooledObject.transform.parent = Scene.transform;
                    return pooledObject;
                }
                else
                {
                    Debug.Log("ObjectPool no encontro el objeto: " + objectType + "  bool " + onlyPooled);
                    GetObjectForType(objectType, onlyPooled);
                }
            }
            if (!onlyPooled)
            {
                SceneObject ro = Entries[i].Prefab;
                ro = Instantiate(ro) as SceneObject;
                ro.transform.parent = Scene.transform;
                return ro;
            }
        }
        return null;
    }




    public void PoolObject(SceneObject obj)
    {
		if (obj.gameObject.name == "bloodx1_real" || obj.gameObject.name == "bloodx1_real(Clone)") {
			print ("SI bloodx1_real");
			obj.gameObject.SetActive (true);
		}
		
        for (int i = 0; i < Entries.Length; i++)
        {
            if (Entries[i].Prefab.name == obj.name || Entries[i].Prefab.name + "(Clone)" == obj.name)
            {        
				if (obj.gameObject.name == "bloodx1_real")
					print ("????? bloox1_real");
				
				obj.transform.SetParent(containerObject.transform);
				obj.gameObject.SetActive(false);
                pooledObjects[i].Add(obj);
                return;
            }
        }
        if (obj.name == "extralargeBlock1_real" || obj.name == "extraSmallBlock1_real(Clone)")
        {
            Debug.LogError("rompe un extralarge block_________________________________");
        }
        Destroy(obj.gameObject);
    }



}