using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Missions : MonoBehaviour {

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

    private Text name_txt;
    private Text desc_txt;
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
        
    }
    void OnDestroy()
    {
        Data.Instance.events.OnScoreOn -= OnScoreOn;
        Data.Instance.events.OnGrabHeart -= OnGrabHeart;
    }

    public void OnDisable()
    {
        data.events.OnListenerDispatcher -= OnListenerDispatcher;
    }
    private void OnListenerDispatcher(string message)
    {        
       if (message == "ShowMissionName")
            activateMissionByListener();        
    }
	public void Init (int _MissionActiveID, Level level) {

       
        data.events.OnListenerDispatcher += OnListenerDispatcher;
        state = states.INACTIVE; 

		this.missionCompletedPercent = 0;
		this.MissionActiveID = _MissionActiveID-1;

        this.level = level;
        progressBar = level.missionBar;

        name_txt = level.missionName;
        desc_txt = level.missionDesc;

#if UNITY_EDITOR
        if (data.DEBUG && test_mission)
        {
            print("___________");
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
                print("____TERMINO LA MISION EN MODO COMPETENCIA");
                MissionActiveID = 0;
                MissionActive.reset();
                desc_txt.text = "CORRE ";
                return;
            }
            else
            if (MissionActiveID == GetActualMissions().Length)
            {
                MissionActiveID = Random.Range(2, GetActualMissions().Length - 1);
            }
        }
        MissionActive = GetActualMissions()[MissionActiveID];
		MissionActive.reset();
		MissionActiveID++;
	}
    private void activateMissionByListener()
    {

        state = states.ACTIVE;
        if (Data.Instance.playMode == Data.PlayModes.COMPETITION)
        {
            if(!Data.Instance.isArcade)
                desc_txt.text = "CORRE " + MissionActive.distance + " METROS";
        } else 
        if (MissionActive.Hiscore > 0)
        {
            name_txt.text = MissionActive.avatarHiscore;
            desc_txt.text = "SCORE: " + MissionActive.Hiscore; 
        }
        else
        {
            name_txt.text = "MISSION " + MissionActiveID;
            desc_txt.text = MissionActive.description.ToUpper();
        }
        
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
	public void killBomb(int qty) {
		if(MissionActive.bombs > 0)
		{
            addPoints(qty);
			setMissionStatus(MissionActive.bombs);
		}
	}

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
        if (Data.Instance.isArcade) return;
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
               // level.Complete();
            }
            
		}
	}
}
