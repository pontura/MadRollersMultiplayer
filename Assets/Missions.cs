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

	public Mission[] missions;
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
    private int lastDistance = 0;
    private int distance;

    public void Init()
    {
		lastMissionSignalShowed = -1;
        data = Data.Instance;

		int videogameID = 0;
		int lastVideoGameID = -1;
			
		List<MissionButton> all = new List<MissionButton> ();
		int id = 0;
		foreach (Mission mission in missions) {
			mission.id = id;
			if (lastVideoGameID != mission.videoGameID) {
				lastVideoGameID = mission.videoGameID;


				Missions.MissionsByVideogame mbv = new Missions.MissionsByVideogame ();
				allMissionsByVideogame.Add (mbv);
				mbv.missions = new List<Mission> ();

			} 

			videogameID = mission.videoGameID;
			allMissionsByVideogame [videogameID].missions.Add (mission);
			id++;
		}
    }
    private void OnListenerDispatcher(string message)
    {        
       if (message == "ShowMissionName")
            activateMissionByListener();        
    }
	public void Init (int _MissionActiveID, Level level) {
      
        state = states.INACTIVE; 

		MissionActiveID = _MissionActiveID;
		MissionActive = missions [MissionActiveID];

		Data.Instance.events.OnChangeBackgroundSide (MissionActive.backgroundSides);

		this.missionCompletedPercent = 0;

        this.level = level;
        progressBar = level.missionBar;

#if UNITY_EDITOR
        if (data.DEBUG && test_mission)
        {
            MissionActive = test_mission;
            MissionActive.reset();
            return;
        }
#endif
		if (Data.Instance.playMode == Data.PlayModes.COMPETITION && 1==2)
        {
           // MissionActiveID = 0;
            MissionActive = Data.Instance.competitions.competitions[0].missions[0];
            MissionActive.reset();
            MissionActiveID = 0;  
        } else
        {
            MissionActive = GetActualMissions()[MissionActiveID];
            MissionActive.reset();
        }


	}
    public Mission[] GetActualMissions()
    {
		if (Data.Instance.playMode == Data.PlayModes.COMPETITION  && 1==2)
            return competitions.GetMissions();
        else return missions;

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
//        if (Data.Instance.isArcade)
//        {
//            MissionActiveID = 0;
//            MissionActive.reset();
//        } else
//        {
			if (Data.Instance.playMode == Data.PlayModes.COMPETITION  && 1==2)
            {
                MissionActiveID = 0;
                MissionActive.reset();
               //desc_txt.text = "CORRE ";
				return false;
            }
            else
            if (MissionActiveID >= GetActualMissions().Length-1)
            {
				Game.Instance.GotoVideogameComplete ();
				return false;
				MissionActiveID = UnityEngine.Random.Range(2, GetActualMissions().Length - 1);
            }
      //  }
		MissionActiveID++;
        MissionActive = GetActualMissions()[MissionActiveID];
		Data.Instance.events.OnChangeBackgroundSide (MissionActive.backgroundSides);
		MissionActive.reset();
		data.events.NewMissionStart ();
		return true;
	}
    private void activateMissionByListener()
    {

        state = states.ACTIVE;
		string text = "";

        if (MissionActive.Hiscore > 0)
			text = "SCORE: " + MissionActive.Hiscore; 
        else
			text = MissionActive.description.ToUpper();
        
        //MissionActive.points = 0;
        lastDistance = (int)Game.Instance.GetComponent<CharactersManager>().distance;
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
		return missions[MissionActiveID];
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

//	private void OnScoreOn(int playerID, Vector3 pos, int qty, ScoresManager.types type)
//    {
//		if (!CanComputeMission ())
//			return;
//        if (MissionActive.Hiscore > 0)
//        {
//            addPoints(qty);
//            setMissionStatus(MissionActive.Hiscore);
//        }
//    }
//    //lo llama el player
//    public void updateDistance(float qty)
//    {
//		if (!CanComputeMission ())
//			return;
//        if (state == states.INACTIVE) return;
//        distance = (int)qty - lastDistance;
//        if (MissionActive.distance > 0)
//        {
//            setPoints(distance);
//            setMissionStatus(MissionActive.distance);
//        }
//    }
//	public void hitBoss (int qty) {
//
//		print ("hitBoss " + qty + "   MissionActive.boss1 " + MissionActive.boss1);
//		if (!CanComputeMission ())
//			return;
//		if(MissionActive.boss1 > 0)
//		{
//			addPoints(qty);		
//			setMissionStatus(MissionActive.boss1);
//		}
//	}
//	public void killGuy (int qty) {
//		if (!CanComputeMission ())
//			return;
//		if(MissionActive.guys > 0)
//		{
//            addPoints(qty);		
//			setMissionStatus(MissionActive.guys);
//		}
//	}
//	public void killPlane() {
//		if (!CanComputeMission ())
//			return;
//		if(MissionActive.planes > 0)
//		{
//            addPoints(1);		
//			setMissionStatus(MissionActive.planes);
//		}
//	}
//	public void OnDestroySceneObject(string name)
//	{
//		if (!CanComputeMission ())
//			return;
//		print ("name: " + name);
//		if(name == "bomb" && MissionActive.bombs > 0)
//		{
//			addPoints(1);
//			setMissionStatus(MissionActive.bombs);
//		} else if(name == "ghost" && MissionActive.ghost > 0)
//		{
//			addPoints(1);
//			setMissionStatus(MissionActive.ghost);
//		} else if(name == "boss1" && MissionActive.boss1 > 0)
//		{
//			addPoints(1);
//			setMissionStatus(MissionActive.boss1);
//		} 
//	}
//    void OnGrabHeart()
//    {
//		if (!CanComputeMission ())
//			return;
//		if(MissionActive.hearts > 0)
//		{
//            addPoints(1);
//			setMissionStatus(MissionActive.hearts);
//		}
//	}
//    void addPoints(float qty)
//    {
//		if (!CanComputeMission ())
//			return;
//        if (state == states.INACTIVE) return;
//        MissionActive.addPoints(qty);
//    }
//    void setPoints(float points)
//    {
//        if (state == states.INACTIVE) return;
//        MissionActive.setPoints((int)points);
//    }
//	void setMissionStatus(int total)
//	{
//		if (Data.Instance.playMode != Data.PlayModes.STORY && Data.Instance.playMode != Data.PlayModes.COMPETITION)
//			return;
//        if (state == states.INACTIVE) return;
//
//		missionCompletedPercent = MissionActive.points * 100 / total;
//
//		progressBar.setProgression(missionCompletedPercent);
//
//		if (MissionActive.distance == 0)
//			Data.Instance.events.OnMissionProgres ();
//
//		if(missionCompletedPercent >= 100)
//		{
//            progressBar.reset();
//			if (Data.Instance.playMode == Data.PlayModes.COMPETITION  && 1==2)
//            {
//                Data.Instance.events.OnCompetitionMissionComplete();
//            }
//            else
//            {
//                lastDistance = distance;
//                level.Complete();
//            }            
//		}
//	}

}
