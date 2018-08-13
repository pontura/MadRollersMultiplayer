using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObjectsManager : MonoBehaviour {

	public CharactersManager charactersManager;
	List<SceneObject> sceneObjectsInScene;
	private ObjectPool Pool;

	void Awake()
	{
		Pool = Data.Instance.sceneObjectsPool;
		sceneObjectsInScene = new List<SceneObject>();
	}
	public void AddSceneObject(SceneObject so, Vector3 pos)
	{
		so.gameObject.SetActive (false);
		so.isActive = false;
		so.transform.SetParent(Pool.Scene.transform);
		so.transform.localPosition = pos;
		sceneObjectsInScene.Add (so);
		so.Init (this);
	}
	public void AddSceneObjectAndInitIt(SceneObject so, Vector3 pos)
	{
		so.gameObject.SetActive (false);
		so.isActive = true;
		so.transform.SetParent(Pool.Scene.transform);
		so.transform.localPosition = pos;
		sceneObjectsInScene.Add (so);
		so.Init (this);
		so.Restart (pos);
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
			if (so == null) {
				sceneObjectsInScene.RemoveAt (i);
			} else if (so.transform.localPosition.y < -4) {
				so.Pool ();
			}else if (distance > so.transform.position.z + so.size_z + 22 && Data.Instance.playMode != Data.PlayModes.VERSUS)
				so.Pool();
			else if (distance > so.transform.position.z - 48 || Data.Instance.playMode == Data.PlayModes.VERSUS) {				
				if (!so.isActive) {
					so.Restart (so.transform.position);
				} else {				
					so.Updated (distance);
				}
			}			
		}
	}
	public void PoolSceneObjectsInScene()
	{
		int i = sceneObjectsInScene.Count;
		while (i>0) {
			SceneObject sceneObject = sceneObjectsInScene [i-1];
			sceneObject.Pool();
			i--;
		}
	}
}
