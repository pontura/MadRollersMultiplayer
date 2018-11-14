﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : SceneObject {

	public float distance_from_avatars;
	int hits;
	//Missions missions;
	int totalHits;

	public override void OnRestart(Vector3 pos)
	{		
		Data.Instance.events.OnBossActive (true);
		Data.Instance.GetComponent<MusicManager> ().BossMusic (true);
		base.OnRestart (pos);
	}
	public void SetTotal(int totalHits)
	{		
		this.totalHits = totalHits;
		Data.Instance.events.OnBossInit (totalHits);
	}
	public virtual void OnSceneObjectUpdated()
	{
		
	} 
	public bool HasOnlyOneLifeLeft()
	{
		if (hits+1 >= totalHits)
			return true;
		else
			return false;
	}
	public void breakOut()
	{
		Data.Instance.events.OnSoundFX("FX break", -1);
		hits++;
		Hit ();

		Data.Instance.events.OncharacterCheer ();

		if (hits >= totalHits)
			Killed ();
		else
			Data.Instance.events.OnBossHitsUpdate (hits);
		
	}
	public void Killed()
	{
		Data.Instance.events.OnSoundFX("FX explot00", -1);
		Death ();
		Data.Instance.events.AddExplotion (transform.position, Color.white);
		Invoke ("Died", 0.2f);
	}
	void Died()
	{
		Data.Instance.GetComponent<MusicManager> ().BossMusic (false);
		Game.Instance.level.Complete ();
		Pool ();
		Data.Instance.events.OnBossActive (false);
	}
	public virtual void Hit()
	{
		//Invoke ("Continue", 1);
	}
	public virtual void Death()
	{
		//bossBar.enabled = false;
	}
	public virtual void OnPartBroken(BossPart part) { }
}
