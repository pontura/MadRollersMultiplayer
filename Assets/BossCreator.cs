using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCreator : Boss {
	
	public GameObject assets_to_instantiate;
	public float time_to_init_enemies;

	//[HideInInspector]
	public BossPart[] parts;
	BossSettings settings;

	public override void OnRestart(Vector3 pos)
	{		
		
		settings = GetComponent<BossSettings> ();

		Data.Instance.events.OnBossSetNewAsset (settings.asset);
		Data.Instance.events.OnBossSetTimer (settings.time_to_kill);

		distance_from_avatars = settings.distance_from_avatars;
		time_to_init_enemies = settings.time_to_init_enemies;
		//print ("boss module " + settings.bossModule);

		GameObject assets = Instantiate(Resources.Load("bosses/modules/" + settings.bossModule, typeof(GameObject))) as GameObject;
		//GameObject assets = Instantiate (settings.assets);
		assets.transform.SetParent (transform);
		assets.transform.localPosition = Vector3.zero;
		parts = assets.GetComponentsInChildren<BossPart> ();

		foreach (BossPart part in parts) {
			part.gameObject.SetActive (false);
		}

		SetTotal (parts.Length);
		Init ();

		base.OnRestart (pos);
	}
	public override void OnSceneObjectUpdated()
	{
		float avatarsDistance = Game.Instance.level.charactersManager.getDistance ();
		if (avatarsDistance + distance_from_avatars < transform.localPosition.z)
			return;
		float _z = avatarsDistance + distance_from_avatars;

		Vector3 pos = transform.localPosition;
		pos.z = _z;
		transform.localPosition = pos;
	} 
	int partID;
	void Init()	
	{
		if (partID >= parts.Length)
			return;
		parts [partID].gameObject.SetActive (true);
		parts [partID].Init (this, settings.asset);

		Invoke ("Init", time_to_init_enemies);
		partID++;
	}
	public override void OnPartBroken(BossPart part)
	{
		breakOut ();
	}

}