using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObjectsManager : MonoBehaviour {

	public CharactersManager charactersManager;
	List<SceneObject> sceneObjectsInScene;

	void Start()
	{
		sceneObjectsInScene = new List<SceneObject>();
	}
	public void AddSceneObject(SceneObject so, Vector3 pos)
	{
		so.isActive = false;
		so.transform.localPosition = pos;
		sceneObjectsInScene.Add (so);

		print("added: " + so.name);
	}
	public void RemoveSceneObject(SceneObject so)
	{
		sceneObjectsInScene.Remove (so);
	}
	void LateUpdate () {

		float distance = charactersManager.getDistance();
		int i = sceneObjectsInScene.Count;
		while (i>0) {
			SceneObject so = sceneObjectsInScene [i-1];
			i--;
			if (so.transform.localPosition.y < -6)
				so.Pool();
			else if (distance > so.transform.position.z + so.size_z + 22 && Data.Instance.playMode != Data.PlayModes.VERSUS)
				so.Pool();
			else if (distance > so.transform.position.z - 45 || Data.Instance.playMode == Data.PlayModes.VERSUS) {
				
				if (!so.isActive) {
					so.Init (this, so.transform.position);
				} else {				
					so.Updated (distance);
				}
			}
			
		}
	}
}
