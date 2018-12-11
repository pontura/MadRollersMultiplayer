using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LevelSelector : MonoBehaviour {

	public Animation camAnimnation;

	public GameObject computerUI;
	public Text title;
	public Text credits;
	public MissionButton diskette;
	VideogameData videogameData;
	public int videgameID;
	VideogamesUIManager videogameUI;
	bool canInteract;
	float timePassed;
	MissionSelector missionSelector;
	void Start()
	{		
		Data.Instance.isReplay = false;
		missionSelector = GetComponent<MissionSelector> ();
		Data.Instance.multiplayerData.ResetAll ();
		Data.Instance.events.OnResetScores ();
		timePassed = 0;
		title.text = "SELECT GAME";

		videgameID = Data.Instance.videogamesData.actualID;

		videogameUI = GetComponent<VideogamesUIManager> ();
		videogameUI.Init ();
		SetSelected ();

		Data.Instance.events.OnJoystickClick += OnJoystickClick;
		Data.Instance.events.OnJoystickDown += OnJoystickDown;
		Data.Instance.events.OnJoystickUp += OnJoystickUp;
		Data.Instance.events.OnJoystickLeft += OnJoystickLeft;
		Data.Instance.events.OnJoystickRight += OnJoystickRight;
		Invoke ("SetCanInteract", 1);
		Invoke ("TimeOver", 90);
	}
	void SetCanInteract()
	{
		canInteract = true;
	}
	void TimeOver()
	{
		Data.Instance.LoadLevel("MainMenu");
	}
	void OnDestroy()
	{
		Data.Instance.events.OnJoystickClick -= OnJoystickClick;
		Data.Instance.events.OnJoystickDown -= OnJoystickDown;
		Data.Instance.events.OnJoystickUp -= OnJoystickUp;
		Data.Instance.events.OnJoystickLeft -= OnJoystickLeft;
		Data.Instance.events.OnJoystickRight -= OnJoystickRight;
	}
	void OnJoystickClick()
	{
		if (!canInteract)
			return;
		
		canInteract = false;
		//computerUI.SetActive (false);
		diskette.SetOn ();
		Invoke ("Delayed", 4f);
		camAnimnation.Play ("levelSelectorCamera");
	}
	void Delayed()
	{
		Data.Instance.videogamesData.actualID = videgameID;
		Data.Instance.LoadLevel ("Game");
	}
	void OnJoystickUp()
	{
		print ("up");
		if (!canInteract)
			return;
		
		int MissionActiveID = Data.Instance.missions.MissionActiveID;
		if (MissionActiveID < Data.Instance.missions.GetMissionsByVideoGame (videgameID).missionUnblockedID) {
			Data.Instance.missions.MissionActiveID++;
			missionSelector.ChangeMission (Data.Instance.missions.MissionActiveID);
		}
	}
	void OnJoystickDown()
	{
		print ("down");

		if (!canInteract)
			return;

		int MissionActiveID = Data.Instance.missions.MissionActiveID;
		if (MissionActiveID > 0) {
			Data.Instance.missions.MissionActiveID--;
			missionSelector.ChangeMission (Data.Instance.missions.MissionActiveID);
		}
	}
	void OnJoystickLeft()
	{		
		if (!canInteract)
			return;

		int total =  Data.Instance.videogamesData.all.Length-1;
		if (videgameID < total)
			videgameID++;
		else
			return;
		SetSelected ();
	}
	void OnJoystickRight()
	{
		if (!canInteract)
			return;
		if(videgameID>0)
			videgameID--;
		else
			return;

		SetSelected ();	
	}
	void SetSelected()
	{
		videogameData = Data.Instance.videogamesData.all [videgameID];
		missionSelector.LoadVideoGameData (videgameID);
		diskette.Init (videogameData);
		videogameUI.Change ();
		credits.text = videogameData.credits;
	}
	public void OnJoystickBack()
	{
		Data.Instance.LoadLevel("MainMenu");
	}
}
