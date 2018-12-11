using System;
using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Missions : MonoBehaviour {

	public bool reloadMissions;

	public List<MissionsByVideoGame> videogames;
	[Serializable]
	public class MissionsByVideoGame
	{
		public List<MissionsData> missions;
		public int missionUnblockedID;
	}
	[Serializable]
	public class MissionsData
	{
		public string title;
		public List<MissionData> data;
	}

	public int MissionActiveID = 0;

	[HideInInspector]
	public MissionData MissionActive;
	private float missionCompletedPercent = 0;

	private Level level;
	private bool showStartArea;
	private Data data;
	float distance;

	public AreaData areaDataActive;
	float areasLength;
	int offset = 120;
	int areaSetId = 0;
	int areaNum = 0;
	int areaID = 0;

	VideogamesData videogamesData;

	public void Init()
	{			
		videogamesData = GetComponent<VideogamesData> ();
		data = Data.Instance;

		if (reloadMissions)
			LoadAll ();
		
		data.events.OnMissionComplete += OnMissionComplete;
	}
	public void LoadAll()
	{
		for (int a = 0; a < 3; a++) {
			MissionsByVideoGame videogame = videogames [a];
			videogame.missions = new List<MissionsData> ();
			int videogameID = a + 1;
			for (int b = 0; b < 20; b++) {				
				TextAsset asset = Resources.Load ("missions/" + videogameID + "_" + b) as TextAsset;
				if (asset != null ) {					
					MissionsData missionData = JsonUtility.FromJson<MissionsData> (asset.text);
					videogame.missions.Add (missionData);
				}
			}
			videogame.missionUnblockedID = PlayerPrefs.GetInt("missionUnblockedID_" + videogameID, 0);
		}
	}
	public void Init (Level level) {
		this.level = level;
		areasLength = -4;
		StartNewMission ();
		AddAreaByName ("start_Multiplayer");
	}
	void OnMissionComplete(int id)
	{
		if (MissionActiveID >= videogames [videogamesData.actualID].missions.Count - 1) 
			Game.Instance.GotoVideogameComplete ();
		else
			NextMission();

		videogames [videogamesData.actualID].missionUnblockedID = MissionActiveID;
		int videogameID = videogamesData.actualID+1;
		PlayerPrefs.SetInt ("missionUnblockedID_" + videogameID, MissionActiveID);

		print ("graba MissionActiveID " + MissionActiveID + "missionUnblockedID_" + videogameID);
	}
	public int GetTotalMissionsInVideoGame(int videogameID)
	{
		return videogames [videogameID].missions.Count;
	}
	public MissionsByVideoGame GetMissionsByVideoGame(int videogameID)
	{
		return videogames [videogameID];
	}
	void NextMission()
	{
		AddAreaByName ("newLevel_playing");
		MissionActiveID++;
		StartNewMission ();
		Data.Instance.events.OnChangeBackgroundSide (MissionActive.fondo);
	}
	void StartNewMission()
	{
		areaSetId = 0;
		ResetAreaSet ();
		MissionActive = videogames[videogamesData.actualID].missions[MissionActiveID].data[0];
		this.missionCompletedPercent = 0;
	}
	public MissionData GetActualMissionData()
	{
		return videogames[videogamesData.actualID].missions[MissionActiveID].data[0];
	}
	public MissionData GetMission(int videoGameID, int missionID)
	{
		return videogames[videoGameID].missions[missionID].data[0];
	}
//	public AreasManager getAreasManager()
//	{
//		return null;//MissionActive.GetComponent<AreasManager>();
//	}
	void Complete()
	{
		data.events.MissionComplete();     
	}
	bool CanComputeMission()
	{
		//if (Data.Instance.playMode == Data.PlayModes.STORY || Data.Instance.playMode == Data.PlayModes.COMPETITION)
			return true;
	//	return false;
	}

	public int GetActualMissionByVideogame()
	{
		int viedogameActive = videogamesData.actualID;
		int id = 0;
		foreach (MissionData mission in videogames[viedogameActive].missions[0].data) {
			if (mission.id == MissionActive.id)
				return id;
			id++;
		}
		return 0;
	}

	public void OnUpdateDistance(float distance)
	{
		if (distance > areasLength-offset) {
			SetNextArea ();
		}
		if (MissionActiveID == 0)
			CheckTutorial (distance);
	}
	int total_areas = 1;
	void SetNextArea()
	{
		CreateCurrentArea ();
		Game.Instance.gameCamera.SetOrientation (MissionActive.areaSetData [areaSetId].cameraOrientation);
		total_areas = MissionActive.areaSetData [areaSetId].total_areas;

		if (MissionActive.areaSetData [areaSetId].randomize && Game.Instance.level.charactersManager.getTotalCharacters()==1) 
			total_areas /= 2;
		
		if (areaNum >= total_areas) {
			if (areaSetId < MissionActive.areaSetData.Count - 1) {
				areaSetId++;
				ResetAreaSet ();
			} else {
				areaNum--;
			}
		}
		areaNum++;
	}
	void ResetAreaSet()
	{
		areaNum = 0;
		areaID = 0;
	}
	private void  CreateCurrentArea()
	{
		MissionData.AreaSetData areaSetData = MissionActive.areaSetData[areaSetId];
		string areaName = GetArea(areaSetData);

		//DEBUG:::::
		if(Data.Instance.testAreaName != "")
			AddAreaByName (Data.Instance.testAreaName);
		else
			AddAreaByName (areaName);
		
	}
	void AddAreaByName(string areaName)
	{
		TextAsset asset = Resources.Load ("areas/" + areaName ) as TextAsset;
		if (asset != null) {					
			areaDataActive = JsonUtility.FromJson<AreaData> (asset.text);
			areasLength += areaDataActive.z_length/2;
			level.sceneObjects.AddSceneObjects (areaDataActive, areasLength);
			//print ("km: " + areasLength + " mission: " + MissionActiveID +  " areaSetId: " + areaSetId + " areaID: " + areaID + " z_length: " + areaDataActive.z_length + " en: areas/" + areaName +  " totalAreas" + total_areas );
			areasLength += areaDataActive.z_length/2;
		} else {
			Debug.LogError ("Loco, no existe esta area: " + areaName + " en Respurces/areas/");
		}

	}
	List<MissionData.AreaSetData> GetActualAreaSetData()
	{
		return MissionActive.areaSetData;
	}
	string GetArea(MissionData.AreaSetData areaSetData)
	{
		if (areaSetData.randomize) {
			areaID++;
			return areaSetData.areas [UnityEngine.Random.Range(0,areaSetData.areas.Count)];
		} else if (areaID < areaSetData.areas.Count - 1) {
			areaID++;
			return areaSetData.areas [areaID-1];
		} else {
			return areaSetData.areas [areaSetData.areas.Count-1];
		}
	}

	int tutorialID = 0;
	void CheckTutorial(float distance)
	{
		if(tutorialID >= 3 || Data.Instance.playMode == Data.PlayModes.VERSUS )
			return;

		if (distance>132 && tutorialID < 1)
		{
			Data.Instance.voicesManager.PlayClip (Data.Instance.voicesManager.tutorials [0].audioClip);
			tutorialID = 1;
		} else if(distance>190 && tutorialID < 2)
		{
			Data.Instance.voicesManager.PlayClip (Data.Instance.voicesManager.tutorials [1].audioClip);
			tutorialID = 2;
		} else if(distance>295 && tutorialID < 3)
		{
			Data.Instance.voicesManager.PlayClip (Data.Instance.voicesManager.tutorials [2].audioClip);
			tutorialID = 3;
		}
	}
}
