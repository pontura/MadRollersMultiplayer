using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Floor : MonoBehaviour
{
	float offset = 40;
	public Transform container;
    public int z_length;
	int new_z_length;
    private bool isMoving;
	public float speed = 2;
	public List<BackgroundSideData> all;
	string lastBackgroundSideName;
    private CharactersManager charactersManager;

    public void Init(CharactersManager charactersManager)
    {
        isMoving = true;
        Data.Instance.events.OnGamePaused += OnGamePaused;
        Data.Instance.events.OnAvatarCrash += OnAvatarCrash;
        Data.Instance.events.OnAvatarFall += OnAvatarCrash;
		Data.Instance.events.OnChangeBackgroundSide += OnChangeBackgroundSide;

        this.charactersManager = charactersManager;

    }
    void OnDestroy()
    {
        Data.Instance.events.OnGamePaused -= OnGamePaused;
        Data.Instance.events.OnAvatarCrash -= OnAvatarCrash;
        Data.Instance.events.OnAvatarFall -= OnAvatarCrash;
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
    void OnAvatarCrash(CharacterBehavior cb)
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
		if (charactersDistance == lastCharactersDistance)
			return;
		
		lastCharactersDistance = charactersDistance;
		pos_z += (Time.deltaTime*speed);
		BackgroundSideData toDelete = null;
		foreach (BackgroundSideData go in all) {

			Vector3 pos = go.transform.localPosition;
			
			if (pos.z < charactersDistance - offset) {
				if (!IsAnActiveBackgroundSide(go))
					toDelete = go;
				else
					go.offset += z_length;
			}
			
			pos.z = charactersDistance + go.offset - pos_z;
			go.transform.localPosition = pos;

		}
		if(toDelete != null)
		{
			all.Remove (toDelete);
			Destroy (toDelete.gameObject);
		}

    }
	bool IsAnActiveBackgroundSide(BackgroundSideData data)
	{
		foreach (BackgroundSideData d in allBackgroundSides) {
			if (d.backgroundSideName == data.backgroundSideName)
				return true;
		}
		return false;
	}
	List<BackgroundSideData> allBackgroundSides;
	void OnChangeBackgroundSide(BackgroundSideData[] all)
	{
		print ("OnChangeBackgroundSide: " + all.Length);
		if (all.Length == 0)
			return;
		allBackgroundSides = new List<BackgroundSideData> ();
		foreach (BackgroundSideData d in all) {
			d.backgroundSideName = d.gameObject.name;
			allBackgroundSides.Add (d);
		}
		int next = 0;
		while(allBackgroundSides.Count<3)
		{			
			if (all.Length == next)
				next = 0;
			allBackgroundSides.Add (all [next]);	
			next++;
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
		BackgroundSideData go = Instantiate (newGO);
		go.transform.SetParent (container);
		go.transform.localPosition = new Vector3(0,0,z_length);
		go.transform.localScale = Vector3.one;
		all.Add(go);
	}
}
