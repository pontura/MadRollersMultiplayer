using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Floor : MonoBehaviour
{
	float offset = 40;
	public Transform container;
    public int z_length;
	int new_z_length;
	public bool isMoving;
	public float speed = 2;
	public List<BackgroundSideData> all;
	string lastBackgroundSideName;
	public CharactersManager charactersManager;
	public VideogameBossPanel videoGameBossPanel;

	void Start()
    {
        isMoving = true;
        Data.Instance.events.OnGamePaused += OnGamePaused;
		Data.Instance.events.OnGameOver += OnGameOver;
		Data.Instance.events.OnChangeBackgroundSide += OnChangeBackgroundSide;
    }
    void OnDestroy()
    {
        Data.Instance.events.OnGamePaused -= OnGamePaused;
		Data.Instance.events.OnGameOver -= OnGameOver;
		Data.Instance.events.OnChangeBackgroundSide -= OnChangeBackgroundSide;
      //  Data.Instance.events.OnChangeMood -= OnChangeMood;
    }
    void OnGamePaused(bool paused)
    {
        isMoving = !paused;
    }
    void OnChangeMood(int id)
    {
		return;
    }
	void OnGameOver()
    {
        isMoving = false;
    }

	float pos_z = 0;
	float lastCharactersDistance;
    void Update()
    {
        if (!isMoving) return;
        if (!charactersManager) return;
		float charactersDistance = charactersManager.getDistance ();
		videoGameBossPanel.transform.localPosition = new Vector3 (0,0,charactersDistance);
		if (charactersDistance == lastCharactersDistance)
			return;
		
		lastCharactersDistance = charactersDistance;
		pos_z += (Time.deltaTime*speed);
		BackgroundSideData toDelete = null;
		foreach (BackgroundSideData go in all) {

			Vector3 pos = go.transform.localPosition;
			
			if (pos.z < charactersDistance - offset) {
//				if (!IsAnActiveBackgroundSide(go))
//					toDelete = go;
//				else
					go.offset += z_length;
			}
			
			pos.z = charactersDistance + go.offset - pos_z;
			go.transform.localPosition = pos;

		}
//		if(toDelete != null)
//		{
//			all.Remove (toDelete);
//			Destroy (toDelete.gameObject);
//		}

    }
//	bool IsAnActiveBackgroundSide(BackgroundSideData data)
//	{
//		foreach (BackgroundSideData d in allBackgroundSides) {
//			if (d.backgroundSideName == data.backgroundSideName)
//				return true;
//		}
//		return false;
//	}
//	List<BackgroundSideData> allBackgroundSides;
	void OnChangeBackgroundSide(BackgroundSideData[] newBgs)
	{
		print ("__________OnChangeBackgroundSide: " + newBgs.Length);
		if (newBgs.Length == 0)
			return;
		if (all.Count>0 && newBgs [0].backgroundSideName == all [0].backgroundSideName) {
			print ("__________mismo backgrouond!");
			return;
		}
		print ("RemoveAllChildsIn");
		int i = container.childCount;
		while (i > 0) {
			print ("DESTROYYYYYY");
			Destroy (container.GetChild(i-1).gameObject);
			i--;
		}
		all.Clear ();

		List<BackgroundSideData> allBackgroundSides = new List<BackgroundSideData> ();
		foreach (BackgroundSideData d in newBgs) {
			allBackgroundSides.Add (d);
		}
		if(allBackgroundSides.Count<3) 
		{
			int next = 0;
			while(allBackgroundSides.Count<3)
			{			
				if (newBgs.Length == next)
					next = 0;
				allBackgroundSides.Add (allBackgroundSides [next]);	
				next++;
			}	
		}

		z_length = 0;
		foreach (BackgroundSideData data in allBackgroundSides) {
			z_length += data.z_length;
			data.offset = z_length;
			AddNewBgSide (data);
		}
	}
	void AddNewBgSide(BackgroundSideData newGO)
	{
		print ("newGO____________" + newGO.backgroundSideName);
		BackgroundSideData go = Instantiate (newGO);
		go.transform.SetParent (container);
		go.transform.localPosition = new Vector3(0,0,z_length);
		go.transform.localScale = Vector3.one;
		all.Add(go);
	}
}
