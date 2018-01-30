using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MissionSignal : MonoBehaviour {

	public GameObject panel;
    public Text[] fields;
    private bool isClosing;
	public MissionIcon missionIcon;

	// Use this for initialization
	void Start () {
        if (Data.Instance.playMode == Data.PlayModes.COMPETITION)
        {
            Destroy(gameObject);
            return;
        }


        Data.Instance.events.OnAvatarCrash += OnAvatarCrash;
        Data.Instance.events.OnAvatarFall += OnAvatarCrash;
        Data.Instance.events.OnMissionComplete += OnMissionComplete;
        Data.Instance.events.OnListenerDispatcher += OnListenerDispatcher;
        SetOff();
	}
    void OnDestroy()
    {
        Data.Instance.events.OnAvatarCrash -= OnAvatarCrash;
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
    }
    private IEnumerator MissionComplete()
    {
        Open("MISIóN COMPLETA!");
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
		Open("MISIóN " +  Data.Instance.GetComponent<Missions>().MissionActiveID);
        CloseAfter(1.5f);
    }
    private void ShowMissionName()
    {
		Missions missions = Data.Instance.GetComponent<Missions> ();
		//print ("LL:" + missions.MissionActiveID + "    desc   " + missions.missions[ missions.MissionActiveID].description) ;
		Mission mission = missions.missions[ missions.MissionActiveID];
		Open( mission.description.ToUpper());
		missionIcon.SetOn (mission);
        CloseAfter(3);
    }
    private void Open(string text)
    {
        SetOn();
        GetComponent<Animation>().Play("missionOpen");
        GetComponent<Animation>()["missionOpen"].normalizedTime = 0;
		foreach(Text f in fields)
       		f.text = text;		
	}
    void CloseAfter(float delay)
    {
        isClosing = true;
        Invoke("Close", delay);
	}
    public void Close()
    {
        //if (!isClosing) return;
        //isClosing = false;
        SetOff();
        GetComponent<Animation>().Play("missionClose");
        GetComponent<Animation>()["missionClose"].normalizedTime = 0;
    }
}
