using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : SceneObject {

	public float distance_from_avatars;
	public BossBar bossBar;
	public float numberOfHits;

	public override void OnRestart(Vector3 pos)
	{
		Data.Instance.GetComponent<MusicManager> ().BossMusic (true);
		bossBar.enabled = true;
		base.OnRestart (pos);
		bossBar.Init (this);
	}
	public virtual void OnSceneObjectUpdated()
	{
	} 
	public void breakOut()
	{
		Data.Instance.events.OnSoundFX("FX break", -1);
		Hit ();
		bossBar.Resta ((10/numberOfHits)/10);
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
		Data.Instance.events.OnDestroySceneObject ("boss1");
		Pool ();
	}
	public virtual void Hit()
	{
		Invoke ("Continue", 1);
	}
	public virtual void Death()
	{
		bossBar.enabled = false;
	}
	public virtual void OnPartBroken(BossPart part) { }
}
