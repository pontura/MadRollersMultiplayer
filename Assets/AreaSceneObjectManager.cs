using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSceneObjectManager : MonoBehaviour {

	public void AddComponentsToJson(AreaSceneObjectData newSOdata, GameObject go)
	{
		FullRotation fullRotation = go.GetComponent<FullRotation> ();
		TimelineAnimation timelineAnimation = go.GetComponent<TimelineAnimation> ();
		BossSettings bossSettings = go.GetComponent<BossSettings> ();
		MoveForward moveForward = go.GetComponent<MoveForward> ();
		SceneObjectData soData = go.GetComponent<SceneObjectData> ();

		if (soData != null) {
			newSOdata.soData = new List<SceneObjectDataGeneric> ();
			SceneObjectDataGeneric data = new SceneObjectDataGeneric ();
			data.size = soData.size;
			data.bumperForce = soData.bumperForce;
			newSOdata.soData.Add (data);
		} 
		if (fullRotation != null) {
			newSOdata.fullRotationData = new List<FullRotationData> ();
			FullRotationData data = new FullRotationData ();
			data.rotateX = fullRotation.rotateX;
			data.rotateY = fullRotation.rotateY;
			data.rotateZ = fullRotation.rotateZ;
			data.speed = fullRotation.speed;
			newSOdata.fullRotationData.Add (data);
		} 
		if (timelineAnimation != null) {
			newSOdata.timelineAnimation = new List<TimelineAnimationData> ();
			TimelineAnimationData data = new TimelineAnimationData ();
			data.timeLineData = timelineAnimation.timeLineData;
			newSOdata.timelineAnimation.Add (data);
		}
		if (bossSettings != null) {
			newSOdata.bossSettings = new List<BossSettingsData> ();
			BossSettingsData data = new BossSettingsData ();
			data.bossModule = bossSettings.bossModule;
			data.asset = bossSettings.asset;
			data.time_to_init_enemies = bossSettings.time_to_init_enemies;
			newSOdata.bossSettings.Add (data);
		}
		if (moveForward != null) {
			newSOdata.moveForward = new List<MoveForwardData> ();
			MoveForwardData data = new MoveForwardData ();
			data.speed = moveForward.speed;
			data.randomSpeedDiff = moveForward.randomSpeedDiff;
			newSOdata.moveForward.Add (data);
		}
	}
	public void AddComponentsToSceneObject(AreaSceneObjectData jsonData, GameObject so) 
	{
		if (jsonData.soData.Count > 0) {
			SceneObjectDataGeneric data = jsonData.soData [0];
			if (data.bumperForce > 0) {
				Bumper newcomponent = so.gameObject.AddComponent<Bumper> ();
				newcomponent.force = data.bumperForce;
			}
		}
		if (jsonData.fullRotationData.Count > 0) {
			FullRotationData data = jsonData.fullRotationData [0];
			FullRotation newcomponent = so.gameObject.AddComponent<FullRotation> ();
			newcomponent.rotateX = data.rotateX;
			newcomponent.rotateY = data.rotateY;
			newcomponent.rotateZ = data.rotateZ;
			newcomponent.speed = data.speed;
		}
		if (jsonData.timelineAnimation.Count > 0) {
			TimelineAnimationData data = jsonData.timelineAnimation [0];
			TimelineAnimation newcomponent = so.gameObject.AddComponent<TimelineAnimation> ();
			newcomponent.timeLineData = data.timeLineData;
		}
		if (jsonData.bossSettings.Count > 0) {
			BossSettingsData data = jsonData.bossSettings [0];
			BossSettings newcomponent = so.gameObject.AddComponent<BossSettings> ();
			newcomponent.bossModule = data.bossModule;
			newcomponent.time_to_init_enemies = data.time_to_init_enemies;
			newcomponent.asset = data.asset;
		}
		if (jsonData.moveForward.Count > 0) {
			MoveForwardData data = jsonData.moveForward [0];
			MoveForward newcomponent = so.gameObject.AddComponent<MoveForward> ();
			newcomponent.speed = data.speed;
			newcomponent.randomSpeedDiff = data.randomSpeedDiff;
		}
	}
}
