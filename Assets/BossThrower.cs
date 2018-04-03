using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossThrower : Boss {

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
	public void AddEnemy(Vector3 pos)
	{
		pos.x = 0;
		pos.z -= 0;
		pos.y += 0.35f;
		SceneObject sceneObject = Data.Instance.sceneObjectsPool.GetObjectForType( "flyer_real", false);  
		if (sceneObject) {
			sceneObject.isActive = false;
			sceneObject.Restart(pos);
		}
	}

}
