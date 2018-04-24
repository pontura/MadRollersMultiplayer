using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CascadeSceneObject : SceneObject {

	int num = 0;
	public override void OnRestart(Vector3 pos)
	{
		base.OnRestart (pos);
		num = 0;
	}
	public void OnSceneObjectUpdated()
	{
		if (num > 100)
			return;
		SceneObject newSceneObject = Data.Instance.sceneObjectsPool.GetObjectForType("ThrowableSceneObject_real", false);  
		newSceneObject.OnRestart (transform.position);
		newSceneObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
		Vector3 f = transform.forward * 100;
		f.y = Random.Range (-5, 5);
		newSceneObject.GetComponent<Rigidbody> ().AddForce (f,ForceMode.Impulse);
		num++;
	}
}
