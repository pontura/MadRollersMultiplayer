using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MissionSignal : MonoBehaviour {

	public GameObject panel;
    public Text[] fields;
	public Text[] fieldsMissionNum;
    private bool isClosing;

	public GameObject specialIcon_Tutorial1;
	public GameObject specialIcon_Tutorial2;
	public GameObject specialIcon_Tutorial3;

	public Gui gui;

	// Use this for initialization
	void Start () {
        if (Data.Instance.playMode == Data.PlayModes.COMPETITION)
        {
            Destroy(gameObject);
            return;
        }


        Data.Instance.events.OnAvatarCrash += OnAvatarCrash;
		Data.Instance.events.OnShowTutorial += OnShowTutorial;
        Data.Instance.events.OnAvatarFall += OnAvatarCrash;
        Data.Instance.events.OnMissionComplete += OnMissionComplete;
        Data.Instance.events.OnListenerDispatcher += OnListenerDispatcher;
        SetOff();
	}
    void OnDestroy()
    {
        Data.Instance.events.OnAvatarCrash -= OnAvatarCrash;
		Data.Instance.events.OnShowTutorial -= OnShowTutorial;
        Data.Instance.events.OnAvatarFall -= OnAvatarCrash;
        Data.Instance.events.OnListenerDispatcher -= OnListenerDispatcher;
        Data.Instance.events.OnMissionComplete -= OnMissionComplete;
    }
    void OnAvatarCrash(CharacterBehavior cb)
    {
        SetOff();
    }
    private void OnMissionComplete(int levelID)
    {
        SetOff();
    }
    void SetOff()
    {
		panel.SetActive (false);
    }
    void SetOn()
    {
		panel.SetActive (true);
		Time.timeScale = 0.01f;	
    }
    private IEnumerator MissionComplete()
    {
        Open("MISIóN COMPLETA!", -1);
        yield return new WaitForSeconds(1.5f);
        //GetComponent<AudioSource>().Play();
        CloseAfter(1);
	}
    private void OnListenerDispatcher(string message)
    {
        isClosing = false;
        if (message == "ShowMissionId")
            MissionSignalOn();
        else if (message == "ShowMissionName")
            ShowMissionName();        
    }
    private void MissionSignalOn()
    {
		Open("MISIóN " +  Data.Instance.GetComponent<Missions>().MissionActiveID, -1);
        CloseAfter(2f);
    }
	void OnShowTutorial(int id)
	{
		print ("OnShowTutorial " + id);
		Missions missions = Data.Instance.GetComponent<Missions> ();
		Mission mission = missions.missions[ missions.MissionActiveID];
		if (id == 1) {
			Open ("JUMP", -1);
			gui.missionIcon.SetOn (mission, specialIcon_Tutorial1);
		} else if (id == 2) {
			Open ("DOUBLE JUMP", -1);
			gui.missionIcon.SetOn (mission, specialIcon_Tutorial2);
		} else if (id == 3) {
			Open ("NOW... DESTROY!", -1);
			gui.missionIcon.SetOn (mission, specialIcon_Tutorial3);
		}
		
		CloseAfter(2f);
	}
    private void ShowMissionName()
    {
		Missions missions = Data.Instance.GetComponent<Missions> ();
		//print ("LL:" + missions.MissionActiveID + "    desc   " + missions.missions[ missions.MissionActiveID].description) ;
		Mission mission = missions.missions[ missions.MissionActiveID];
		Open( mission.description.ToUpper(), missions.MissionActiveID);
		gui.missionIcon.SetOn (mission);
        CloseAfter(2f);
    }
	private void Open(string text, int missionId)
    {
        SetOn();
       // GetComponent<Animation>().Play("missionOpen");
       // GetComponent<Animation>()["missionOpen"].normalizedTime = 0;
		missionId += 1;
		foreach(Text f in fields)
       		f.text = text;	
		if (missionId == -1) {
			foreach(Text f in fieldsMissionNum)
				f.text = "";		
		} else {
			foreach(Text f in fieldsMissionNum)
				f.text = "MISSION " + missionId;		
		}	
	}

    void CloseAfter(float delay)
    {
        isClosing = true;
		StartCoroutine (Closing(delay));
	}
	IEnumerator Closing(float delay)
	{
		yield return StartCoroutine(Utils.CoroutineUtil.WaitForRealSeconds (delay));
		Time.timeScale = 1;	
		Close ();
	}
    public void Close()
    {
        SetOff();
		Game.Instance.level.charactersManager.ResetJumps ();
    }
}
