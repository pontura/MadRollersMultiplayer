#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

using UnityEngine;

[CustomEditor(typeof(LevelCreator))]
public class LevelCreatorUI : Editor {
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI ();
		LevelCreator levelCreator = (LevelCreator)target;

		if(GUILayout.Button("Load Area"))
		{
			levelCreator.LoadArea ();
		}
		GUILayout.Space (20);


		if (GUILayout.Button ("Load Mission")) {
			levelCreator.LoadMissions (); 
		}

		GUILayout.Space (20);

		if(GUILayout.Button("Update Missions from json"))
		{
			LevelCreator t = (LevelCreator)target;
			t.UpdateMissions ();
		}
		if(GUILayout.Button("Clear"))
		{
			LevelCreator t = (LevelCreator)target;
			t.Clear ();
		}
		if(GUILayout.Button("Save"))
		{
			LevelCreator t = (LevelCreator)target;
			t.SaveArea ();
		}

		//		if(GUILayout.Button("Show Mission"))
		//		{			
		//			LevelCreator levelCreator = (LevelCreator)target;
		//			levelCreator.ResetAreas ();
		//			MissionData mission = levelCreator.GetMission ();
		////			foreach (AreaSet areaSet in mission.GetComponent<AreasManager> ().areaSets) {
		////				foreach (Area area in areaSet.areas) {
		////					levelCreator.AddArea (area.gameObject, area.z_length);
		////				}
		////			}
		//		}
	}
}
#endif



