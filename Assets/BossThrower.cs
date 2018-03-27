using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossThrower : SceneObject {

	public float distance_from_avatars;
	float timeToCreateEnemy = 0.8f;
	float actualTime = 0;
	public BossBar bossBar;
	public float numberOfHits;
	public float timer;
	public states state;
	public Animator anim;
	public List<BossPart> parts;

	public enum states
	{
		PLAYING,
		DEATH
	}

	public override void OnRestart(Vector3 pos)
	{
		Data.Instance.GetComponent<MusicManager> ().BossMusic (true);
		bossBar.enabled = true;
		base.OnRestart (pos);
		//bossBar.Init (this);
		foreach (BossPart part in this.GetComponentsInChildren<BossPart>()) {
			parts.Add (part);
		}
	}
	public void OnSceneObjectUpdated()
	{
		if (state == states.DEATH)
			return;
		float avatarsDistance = Game.Instance.level.charactersManager.getDistance ();
		if (avatarsDistance + distance_from_avatars < transform.localPosition.z)
			return;

		float _z = avatarsDistance + distance_from_avatars;
		Vector3 pos = transform.localPosition;
		pos.z = _z;
		transform.localPosition = pos;

		timer += Time.deltaTime;

	} 
	public void OnPartBroken(BossPart part)
	{
		parts.Remove (part);
	}
	public void breakOut()
	{
		Data.Instance.events.OnSoundFX("FX break", -1);
	//	bossBar.Resta ((10/numberOfHits)/10);
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
		Data.Instance.events.OnDestroySceneObject ("boss2");
		Pool ();
	}
	public void AddEnemy(Vector3 pos)
	{
		pos.z -= 3;
		SceneObject sceneObject = Data.Instance.sceneObjectsPool.GetObjectForType( "flyer_real", false);  
		if (sceneObject) {
			sceneObject.isActive = false;
			sceneObject.Restart(pos);
		}
	}
	void Death()
	{
		bossBar.enabled = false;
		state = states.DEATH;
		anim.Play ("death");
	}
}
