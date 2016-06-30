using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MissionSignal : MonoBehaviour {

	public Image bg;
    public Text field;
    private Missions AllMissions;
    private bool isClosing;

	// Use this for initialization
	void Start () {
        if (Data.Instance.playMode == Data.PlayModes.COMPETITION)
        {
            Destroy(gameObject);
            return;
        }

        AllMissions = Data.Instance.GetComponent<Missions>();

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
        bg.enabled = false;
        field.enabled = false;
    }
    void SetOn()
    {
        bg.enabled = true;
        field.enabled = true;
    }
    private IEnumerator MissionComplete()
    {
        Open("MISIóN COMPLETA!");
        yield return new WaitForSeconds(1.5f);
        GetComponent<AudioSource>().Play();
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
        Open("MISIóN " + AllMissions.MissionActiveID);
        CloseAfter(1.5f);
    }
    private void ShowMissionName()
    {
        Open(AllMissions.missions[AllMissions.MissionActiveID - 1].description.ToUpper());
        CloseAfter(3);
    }
    private void Open(string text)
    {
        SetOn();
        GetComponent<Animation>().Play("missionOpen");
        GetComponent<Animation>()["missionOpen"].normalizedTime = 0;
        field.text = text;		
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
