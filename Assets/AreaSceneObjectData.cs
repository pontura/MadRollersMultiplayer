using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class AreaSceneObjectData  {
	public string name;
	public Vector3 pos;
	public Vector3 rot;
	public List<FullRotationData> fullRotationData;
	public List<TimelineAnimationData> timelineAnimation;
	public List<BossSettingsData> bossSettings;
	public List<MoveForwardData> moveForward;
	public List<SceneObjectDataGeneric> soData;
	public bool isChild;
}
[Serializable]
public class SceneObjectDataGeneric  {
	public Vector3 size;
	public float bumperForce;
}
[Serializable]
public class FullRotationData  {
	public bool rotateX;
	public bool rotateY;
	public bool rotateZ;
	public float speed;
}
[Serializable]
public class TimelineAnimationData  {
	public List<TimelineData> timeLineData;
}
[Serializable]
public class BossSettingsData  {
	public string bossModule;
	public float time_to_init_enemies;
	public string asset;
}
[Serializable]
public class MoveForwardData  {
	public float speed;
	public float randomSpeedDiff;
}

