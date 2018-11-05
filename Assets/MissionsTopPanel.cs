﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MissionsTopPanel : MonoBehaviour
{
    private Animation anim;
	public Text field;

    void Start()
    {
        anim =  GetComponent<Animation>();
		if (Data.Instance.playMode == Data.PlayModes.STORY || Data.Instance.playMode == Data.PlayModes.COMPETITION)
        {
            Data.Instance.events.OnMissionComplete += OnMissionComplete;
            Data.Instance.events.OnListenerDispatcher += OnListenerDispatcher;
			Data.Instance.events.OnMissionProgress += OnMissionProgress;
           // anim.Play("MissionTopOff");
        } else
			anim.Play("MissionTopOff");
    }
    void OnDisable()
    {
        Data.Instance.events.OnListenerDispatcher -= OnListenerDispatcher;
        Data.Instance.events.OnMissionComplete -= OnMissionComplete;
		Data.Instance.events.OnMissionProgress -= OnMissionProgress;
    }
    private void OnMissionComplete(int levelID)
    {
        anim.Play("MissionTopClose");
    }
    private void OnListenerDispatcher(string message)
    {
		if (message == "ShowMissionName") {
			anim.Play ("MissionTopOpen");
			//field.text = Data.Instance.missions.MissionActive.description;
		}
    }
	void OnMissionProgress()
	{
		print ("OnMissionProgres");
		anim.Play ("MissionActive");
	}
}
