using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Missions : MonoBehaviour {


	public Area startingArea;
	public Area[] relaxArea;
	public Area startingAreaDuringGame;

    public Mission test_mission;

	public Mission[] missions;
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
        data = Data.Instance;
        Data.Instance.events.OnScoreOn += OnScoreOn;
        Data.Instance.events.OnGrabHeart += OnGrabHeart;
		Data.Instance.events.OnListenerDispatcher += OnListenerDispatcher;
		Data.Instance.events.OnDestroySceneObject += OnDestroySceneObject;
    }
    void OnDestroy()
    {
        Data.Instance.events.OnScoreOn -= OnScoreOn;
        Data.Instance.events.OnGrabHeart -= OnGrabHeart;
		Data.Instance.events.OnDestroySceneObject -= OnDestroySceneObject;
    }

    public void OnDisable()
    {
      //  data.events.OnListenerDispatcher -= OnListenerDispatcher;
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
        if (Data.Instance.playMode == Data.PlayModes.COMPETITION)
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
        if (Data.Instance.playMode == Data.PlayModes.COMPETITION)
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
	public void StartNext()
	{
        if (Data.Instance.isArcade)
        {
            MissionActiveID = 0;
            MissionActive.reset();
        } else
        {
            if (Data.Instance.playMode == Data.PlayModes.COMPETITION)
            {
                MissionActiveID = 0;
                MissionActive.reset();
               //desc_txt.text = "CORRE ";
                return;
            }
            else
            if (MissionActiveID == GetActualMissions().Length)
            {
                MissionActiveID = Random.Range(2, GetActualMissions().Length - 1);
            }
        }
		MissionActiveID++;
        MissionActive = GetActualMissions()[MissionActiveID];
		MissionActive.reset();

	}
    private void activateMissionByListener()
    {

        state = states.ACTIVE;
		string text = "";
        if (Data.Instance.playMode == Data.PlayModes.COMPETITION)
        {
            if(!Data.Instance.isArcade)
				text = "CORRE " + MissionActive.distance + " METROS";
        } else 
        if (MissionActive.Hiscore > 0)
        {
				//text = MissionActive.avatarHiscore;
				text = "SCORE: " + MissionActive.Hiscore; 
        }
        else
        {
				//text = "MISSION " + MissionActiveID;
				text = MissionActive.description.ToUpper();
        }

		foreach (Text t in Game.Instance.level.missionDesc.GetComponentsInChildren<Text>())
			t.text = text;

        MissionActive.points = 0;
        lastDistance = (int)Game.Instance.GetComponent<CharactersManager>().distance;
    }




    private void OnScoreOn(int playerID, Vector3 pos, int qty)
    {
        if (MissionActive.Hiscore > 0)
        {
            addPoints(qty);
            setMissionStatus(MissionActive.Hiscore);
        }
    }
    //lo llama el player
    public void updateDistance(float qty)
    {
        if (state == states.INACTIVE) return;
        distance = (int)qty - lastDistance;
        if (MissionActive.distance > 0)
        {
            setPoints(distance);
            setMissionStatus(MissionActive.distance);
        }
    }
	public void killGuy (int qty) {
		if(MissionActive.guys > 0)
		{
            addPoints(qty);		
			setMissionStatus(MissionActive.guys);
		}
	}
	public void killPlane() {
		if(MissionActive.planes > 0)
		{
            addPoints(1);		
			setMissionStatus(MissionActive.planes);
		}
	}
	public void OnDestroySceneObject(string name)
	{
		print ("name: " + name);
		if(name == "bomb" && MissionActive.bombs > 0)
		{
			addPoints(1);
			setMissionStatus(MissionActive.bombs);
		} else if(name == "ghost" && MissionActive.ghost > 0)
		{
			addPoints(1);
			setMissionStatus(MissionActive.ghost);
		}
	}
	//public void killBomb(int qty) {
	//	if(MissionActive.bombs > 0)
	//	{
    //        addPoints(qty);
	//		setMissionStatus(MissionActive.bombs);
	//	}
	//}

    void OnGrabHeart()
    {
		if(MissionActive.hearts > 0)
		{
            addPoints(1);
			setMissionStatus(MissionActive.hearts);
		}
	}
    void addPoints(float qty)
    {
        if (state == states.INACTIVE) return;
        MissionActive.addPoints(qty);
    }
    void setPoints(float points)
    {
        if (state == states.INACTIVE) return;
        MissionActive.setPoints((int)points);
    }
	void setMissionStatus(int total)
	{
		if (Data.Instance.playMode != Data.PlayModes.STORY)
			return;
        if (state == states.INACTIVE) return;
		missionCompletedPercent = MissionActive.points * 100 / total;
		progressBar.setProgression(missionCompletedPercent);
		if(missionCompletedPercent >= 100)
		{
            progressBar.reset();
            if (Data.Instance.playMode == Data.PlayModes.COMPETITION)
            {
                Data.Instance.events.OnCompetitionMissionComplete();
            }
            else
            {
                lastDistance = distance;
                level.Complete();
            }
            
		}
	}
	public Mission GetMissionActive()
	{
		return missions[MissionActiveID];
	}
}
