using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : SceneObject {

	public float distance_from_avatars;
	float timeToCreateEnemy = 1.5f;
	float actualTime = 0;
	public BossBar bossBar;
	public float numberOfHits;
	public float timeToConvert;
	public float timer;
	public states state;
	public Move move;
	public ComeClose comeClose;
	public Animator anim;

	public enum states
	{
		MOVEING,
		COME_CLOSE,
		DEATH
	}

	public override void OnRestart(Vector3 pos)
	{
		Data.Instance.GetComponent<MusicManager> ().BossMusic (true);
		bossBar.enabled = true;
		base.OnRestart (pos);
		bossBar.Init (this);
	}
	public void OnSceneObjectUpdated()
	{
		if (state == states.DEATH)
			return;
		float avatarsDistance = Game.Instance.level.charactersManager.getDistance ();
		if (avatarsDistance + distance_from_avatars < transform.localPosition.z)
			return;

		float _z = avatarsDistance + distance_from_avatars;


		timer += Time.deltaTime;
		if (timer >= timeToConvert) {
			if (state == states.MOVEING) {
				ConvertToComeFront ();
			} 
			timer = 0;
		}
		if (state == states.MOVEING) {
			Vector3 pos = transform.localPosition;
			pos.z = _z;
			transform.localPosition = pos;
			actualTime += Time.deltaTime;
			if (actualTime > timeToCreateEnemy) {
				actualTime = 0;
				AddEnemy ();
			}
		} else {
			comeClose.OnUpdate (_z);
		}

	} 
	void ConvertToComeFront()
	{
		actualTime = 0;
		move.enabled = false;
		comeClose.enabled = true;
		comeClose.Init (distanceFromCharacter);
		state = states.COME_CLOSE;
		timer = 0;
		RunAttack ();
	}
	public void ConvertToMove()
	{
		comeClose.enabled = false;
		move.enabled = true;
		state = states.MOVEING;
		timer = 0;
		Run ();
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
	void AddEnemy()
	{
		SceneObject sceneObject = Data.Instance.sceneObjectsPool.GetObjectForType( "enemyFrontal_real", false);  
		if (sceneObject) {
			sceneObject.isActive = false;
			Vector3 pos = transform.position;
			pos.z -= 4;
			pos.y += 2;
			sceneObject.Restart(pos);
			EnemyRunnerBehavior comp = sceneObject.GetComponent<EnemyRunnerBehavior> ();
			if (comp != null)
				comp.speed = 2;
			else {
				EnemyRunnerBehavior newComp = sceneObject.gameObject.AddComponent <EnemyRunnerBehavior>() as EnemyRunnerBehavior;
				newComp.speed = 2;
			}
		}
	}
	void RunAttack()
	{
		gameObject.tag = "firewall";
		anim.Play ("run_attack");
	}
	void Run()
	{
		gameObject.tag = "boss";
		anim.Play ("run");
	}
	void Hit()
	{
		anim.Play ("hit");
		Invoke ("Continue", 1);
	}
	void Continue()
	{
		if (state == states.MOVEING)
			Run ();
		else if (state == states.COME_CLOSE)
			RunAttack ();
	}
	void Death()
	{
		bossBar.enabled = false;
		state = states.DEATH;
		anim.Play ("death");
	}
}
