using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class AreaCreator : MonoBehaviour {

	AreaSceneObjectManager areaSceneobjectManager;
	public AreaData areaData;

	public List<AreaSceneObjectData> data;
	public AreaCreatorSceneObject areaCreatorSceneObject;

	GameObject lastSceneObjectContainer;
	public void AddSceneObjectsToNewArea(string areaName, AreaData areaData, float _length)
	{
		GameObject newArea = new GameObject ();
		Area area = newArea.AddComponent<Area> ();
		area.z_length = areaData.z_length;
		area.totalCoins = areaData.totalCoins;
		newArea.transform.SetParent (transform);
		newArea.name = areaName;
		foreach (AreaSceneObjectData areaSceneObjectData in areaData.data) {
			GameObject go = areaCreatorSceneObject.GetSceneObjectToAdd (areaSceneObjectData);
			if (go != null) {

				if (lastSceneObjectContainer != null && areaSceneObjectData.isChild)
					go.transform.SetParent (lastSceneObjectContainer.transform);
				else
					go.transform.SetParent (newArea.transform);
				

				if (go.name == "Container") {
					lastSceneObjectContainer = go;
				}
				



				go.transform.position = areaSceneObjectData.pos;
				go.transform.eulerAngles = areaSceneObjectData.rot;
			}
		}
		newArea.transform.localPosition = new Vector3 (0, 0, _length);
	}
	public void CreateData(Area areaReal)
	{
		areaSceneobjectManager = GetComponent<AreaSceneObjectManager> ();
		GameObject area = areaReal.gameObject;
		data.Clear ();
		int totalCoins = 0;
		foreach (Transform t in area.GetComponentsInChildren<Transform>()) {
			if (t.tag == "sceneObject") {
				AddSceneObjectToFile (t.gameObject);
				if (t.name == "Coin" || t.name == "bloodx1")
					totalCoins++;
			}

		}
		var a = new AreaData { data = data };
		a.totalCoins = totalCoins;
		a.z_length = areaReal.z_length;
		string json = JsonUtility.ToJson (a);

		using (FileStream fs = new FileStream ("Assets/Resources/areas/" + area.name + ".json", FileMode.Create)) {
			using (StreamWriter writer = new StreamWriter (fs)) {
				writer.Write (json);
			}
		}
	}
	void AddSceneObjectToFile(GameObject go)
	{
		AreaSceneObjectData newSOdata = new AreaSceneObjectData ();
		if ( go.transform.parent.tag == "sceneObject")
			newSOdata.isChild = true;
		newSOdata.name = go.name;
		newSOdata.pos = RoundVector3(go.transform.position);
		newSOdata.rot = RoundVector3(go.transform.eulerAngles);
		areaSceneobjectManager.AddComponentsToJson (newSOdata, go);
		data.Add (newSOdata);
	}
	Vector3 RoundVector3(Vector3 v)
	{
		Vector3 newV = Vector3.zero;
		newV.x = Round( (float)v.x, 2);
		newV.y = Round( (float)v.y, 2);
		newV.z = Round( (float)v.z, 2);
		return newV;
	}
	public float Round(float value, int digits)
	{
		float mult = Mathf.Pow(10.0f, (float)digits);
		return Mathf.Round(value * mult) / mult;
	}
}
