using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Missions : MonoBehaviour {

	public int lastMissionSignalShowed;
	public Area startingAreaLevel1;
	public Area startingArea;
	public Area[] relaxArea;

	public Area startingAreaAfterDying;
	public Area startingAreaDuringGame;

    public Mission test_mission;

	//public Mission[] missions;
	public List<MissionsByVideogame> allMissionsByVideogame;

	[Serializable]
	public class MissionsByVideogame
	{
		public List<Mission> missions;
	}
    public Competitions competitions;
	public int MissionActiveID = 0;

    public Mission MissionActive;
	private float missionCompletedPercent = 0;

	private ProgressBar progressBar;    

    private states state;
    private enum states
    {
        INACTIVE,
        ACTIVE
    }

	private GameObject name_txt;
	private GameObject desc_txt;
	//private Transform background;
	private Level level;
	private bool showStartArea;
    private Data data;
	private float startingDistance;
	float distance;
	bool missionByDistance;
	int totalDistance;

    public void Init()
    {
		lastMissionSignalShowed = -1;
        data = Data.Instance;

		int videogameID = 0;
		int lastVideoGameID = -1;
			
		List<MissionButton> all = new List<MissionButton> ();
		int id = 0;
		print (allMissionsByVideogame [Data.Instance.videogamesData.actualID].missions.Count);

		foreach (Mission mission in allMissionsByVideogame[Data.Instance.videogamesData.actualID].missions) {
			print (mission.name);
		}
		foreach (Mission mission in allMissionsByVideogame[Data.Instance.videogamesData.actualID].missions) {
			mission.id = id;			
			id++;
		}
    }
    private void OnListenerDispatcher(string message)
    {        
       if (message == "ShowMissionName")
            activateMissionByListener();        
    }
	public void Init (int _MissionActiveID, Level level) {
      
	//	progressBar.gameObject.SetActive (false);
        state = states.INACTIVE; 

		MissionActiveID = _MissionActiveID;
		MissionActive = allMissionsByVideogame[Data.Instance.videogamesData.actualID].missions[MissionActiveID];

		Data.Instance.events.OnChangeBackgroundSide (MissionActive.backgroundSides);

		this.missionCompletedPercent = 0;

        this.level = level;
        progressBar = level.missionBar;
		if (Data.Instance.playMode == Data.PlayModes.COMPETITION && 1==2)
        {
            MissionActive = Data.Instance.competitions.competitions[0].missions[0];
            MissionActive.reset();
            MissionActiveID = 0;  
        } else
        {
            MissionActive = GetActualMissions()[MissionActiveID];
            MissionActive.reset();
        }

	}
    public List<Mission> GetActualMissions()
    {
		return allMissionsByVideogame[Data.Instance.videogamesData.actualID].missions;
    }
	public AreasManager getAreasManager()
	{
		return MissionActive.GetComponent<AreasManager>();
	}
	public void Complete()
	{
        data.events.MissionComplete();
        state = states.INACTIVE;        
	}
	public bool StartNext()
	{
		List<Mission> all = GetActualMissions();

		if (Data.Instance.playMode == Data.PlayModes.COMPETITION  && 1==2)
        {
            MissionActiveID = 0;
            MissionActive.reset();
			return false;
        }
        else if (MissionActiveID >= all.Count-1)
        {
			Game.Instance.GotoVideogameComplete ();
			return false;
			MissionActiveID = UnityEngine.Random.Range(2, all.Count - 1);       
		}
		MissionActiveID++;
        MissionActive = GetActualMissions()[MissionActiveID];
		Data.Instance.events.OnChangeBackgroundSide (MissionActive.backgroundSides);
		MissionActive.reset();
		data.events.NewMissionStart ();

//		if (MissionActive.type == Mission.types.DISTANCE) {
//			StartProgressBar ();
//			missionByDistance = true;
//			startingDistance = level.charactersManager.getDistance ();
//			totalDistance = MissionActive.totalDistance;
//		} else
//		{
			StopProgressBar ();
			missionByDistance = false;
	//	}
		return true;
	}
	void StartProgressBar()
	{
		progressBar.gameObject.SetActive (true);
	}
	void StopProgressBar()
	{
		progressBar.gameObject.SetActive (false);
	}
//	void FixedUpdate()
//	{
//		if (!missionByDistance)
//			return;
//
//		distance = level.charactersManager.getDistance () - (float)startingDistance;
//		float value = distance / totalDistance;
//		progressBar.setProgression (value);
//	}
    private void activateMissionByListener()
    {
        state = states.ACTIVE;
//		string text = "";
//
//        if (MissionActive.Hiscore > 0)
//			text = "SCORE: " + MissionActive.Hiscore; 
//        else
//			text = MissionActive.description.ToUpper();
        
//        lastDistance = (int)Game.Instance.GetComponent<CharactersManager>().distance;
    }

	bool CanComputeMission()
	{
		if (Data.Instance.playMode == Data.PlayModes.STORY || Data.Instance.playMode == Data.PlayModes.COMPETITION)
			return true;
		return false;
	}

	public int GetActualMissionByVideogame()
	{
		int viedogameActive = Data.Instance.videogamesData.actualID;
		int id = 0;
		foreach (Mission mission in allMissionsByVideogame[viedogameActive].missions) {
			if (mission.id == MissionActive.id)
				return id;
			id++;
		}
		return 0;
	}
	public Mission GetMissionActive()
	{
		int viedogameActive = Data.Instance.videogamesData.actualID;
		return allMissionsByVideogame[viedogameActive].missions[MissionActiveID];
	}
	public void ResetLastMissionID()
	{
		lastMissionSignalShowed = -1;
	}
	public void SetLastMissionID(int lastMissionSignalShowed)
	{
		this.lastMissionSignalShowed = lastMissionSignalShowed;
	}
	public bool HasBeenShowed(int title)
	{
		if (lastMissionSignalShowed == title)
			return true;
		return false;
	}
	public void ActivateFirstGameByVideogame(int videoGameID)
	{
		MissionActiveID = allMissionsByVideogame[videoGameID].missions[0].id;
	}
	public void ForceBossPercent(int totalHits)
	{
		//MissionActive.boss1 = totalHits;
	}	

}
