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
		SceneObject newSceneObject;
		Vector3 f;
		if (Data.Instance.playMode == Data.PlayModes.VERSUS && num%4==0) {
			 newSceneObject = Data.Instance.sceneObjectsPool.GetObjectForType("ThrowableSceneObject_real", false);  
			newSceneObject.OnRestart (transform.position);
			newSceneObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			f = transform.forward * 100;
			f.y = Random.Range (-5, 5);
			newSceneObject.GetComponent<Rigidbody> ().AddForce (f,ForceMode.Impulse);
			return;
		}
		num++;
		if (num > 100)
			return;
		 newSceneObject = Data.Instance.sceneObjectsPool.GetObjectForType("ThrowableSceneObject_real", false);  
		newSceneObject.OnRestart (transform.position);
		newSceneObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
		f = transform.forward * 100;
		f.y = Random.Range (-5, 5);
		newSceneObject.GetComponent<Rigidbody> ().AddForce (f,ForceMode.Impulse);
	}
}
