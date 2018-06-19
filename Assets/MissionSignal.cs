using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MissionSignal : MonoBehaviour {

	public GameObject panel;
    public Text[] fields;
	public Text[] fieldsMissionNum;
    bool isOn;

	public GameObject specialIcon_Tutorial1;
	public GameObject specialIcon_Tutorial2;
	public GameObject specialIcon_Tutorial3;

	public Gui gui;

	// Use this for initialization
	void Start () {
		if (Data.Instance.playMode == Data.PlayModes.COMPETITION  && 1==2)
        {
            Destroy(gameObject);
            return;
        }


        Data.Instance.events.OnAvatarCrash += OnAvatarCrash;
		Data.Instance.events.OnShowTutorial += OnShowTutorial;
        Data.Instance.events.OnAvatarFall += OnAvatarCrash;
        Data.Instance.events.OnMissionComplete += OnMissionComplete;
        Data.Instance.events.OnListenerDispatcher += OnListenerDispatcher;
		Data.Instance.events.OnAvatarShoot += OnAvatarShoot;
        SetOff();
	}
    void OnDestroy()
    {
        Data.Instance.events.OnAvatarCrash -= OnAvatarCrash;
		Data.Instance.events.OnShowTutorial -= OnShowTutorial;
        Data.Instance.events.OnAvatarFall -= OnAvatarCrash;
        Data.Instance.events.OnListenerDispatcher -= OnListenerDispatcher;
        Data.Instance.events.OnMissionComplete -= OnMissionComplete;
		Data.Instance.events.OnAvatarShoot -= OnAvatarShoot;
    }
    void OnAvatarCrash(CharacterBehavior cb)
    {
        SetOff();
    }
    private void OnMissionComplete(int levelID)
    {
        Invoke("SetOff", 0.1f);
    }
    void SetOff()
    {
		panel.SetActive (false);
    }
	Mission mission;
	void RefreshMissionIcon()
	{
		Missions missions = Data.Instance.GetComponent<Missions> ();
		mission = missions.missions[ missions.MissionActiveID];
		gui.missionIcon.SetOn (mission);
	}
    void SetOn()
    {
		if (Data.Instance.missions.MissionActiveID == 0)
			return;
		int missionID = Data.Instance.GetComponent<Missions> ().MissionActiveID;
		
		if (missionID>0 && Data.Instance.missions.HasBeenShowed (missionID))
			return;

		isOn = true;

		Data.Instance.missions.SetLastMissionID (missionID);
		
		panel.SetActive (true);
		Data.Instance.events.RalentaTo (0.05f, 0.3f);
    }
	void OnAvatarShoot(int id)
	{
		if (!isOn)
			return;

		Data.Instance.events.ForceFrameRate(1);
		Close ();
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
        if (message == "ShowMissionId")
            MissionSignalOn();
        else if (message == "ShowMissionName")
            ShowMissionName();        
    }
    private void MissionSignalOn()
    {
		Open("MISIóN " +  Data.Instance.GetComponent<Missions> ().MissionActiveID, -1);
        CloseAfter(1.5f);
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
		RefreshMissionIcon ();
		Open( mission.description.ToUpper(), mission.id);
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
		if (missionId == 0) {
			foreach(Text f in fieldsMissionNum)
				f.text = "";		
		} else {
			foreach(Text f in fieldsMissionNum)
				f.text = "MISSION " + missionId;		
		}	
		Game.Instance.level.NewMissionAreaStart ();
	}

    void CloseAfter(float delay)
    {
		StartCoroutine (Closing(delay));
	}
	IEnumerator Closing(float delay)
	{
		yield return StartCoroutine(Utils.CoroutineUtil.WaitForRealSeconds (delay));
		Data.Instance.events.RalentaTo (1, 0.05f);
		Close ();
	}
    public void Close()
    {
		isOn = false;
        SetOff();
		Game.Instance.level.charactersManager.ResetJumps ();
    }
}
