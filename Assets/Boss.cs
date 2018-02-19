using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : SceneObject {

	public float distance_from_avatars;
	float timeToCreateEnemy = 1.5f;
	float actualTime = 0;
	public BossBar bossBar;
	public float numberOfHits;

	public override void OnRestart(Vector3 pos)
	{
		base.OnRestart (pos);
		bossBar.Init (this);
	}
	public void OnSceneObjectUpdated()
	{
		float avatarsDistance = Game.Instance.level.charactersManager.getDistance ();
		if (avatarsDistance + distance_from_avatars < transform.localPosition.z)
			return;
		
		float _z = avatarsDistance + distance_from_avatars;
		Vector3 pos = transform.localPosition;
		pos.z = _z;
		transform.localPosition = pos;

		actualTime += Time.deltaTime;

		if (actualTime > timeToCreateEnemy) {
			actualTime = 0;
			AddEnemy ();
		}
	} 
	public void breakOut()
	{
		print ("BREAK");
		bossBar.Resta ((10/numberOfHits)/10);
	}
	public void Killed()
	{
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
}
