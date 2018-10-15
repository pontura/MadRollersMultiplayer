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
	public MissionButton diskette;
	VideogameData videogameData;
	public int videgameID;
	VideogamesUIManager videogameUI;
	bool canInteract;
	float timePassed;
	void Start()
	{		
		Data.Instance.multiplayerData.ResetAll ();
		Data.Instance.events.OnResetScores ();
		timePassed = 0;
		title.text = "SELECT GAME";

		videgameID = Data.Instance.videogamesData.actualID;

		videogameUI = GetComponent<VideogamesUIManager> ();
		videogameUI.Init ();
		SetSelected ();

		Data.Instance.events.OnJoystickClick += OnJoystickClick;
		//Data.Instance.events.OnJoystickDown += OnJoystickDown;
		//Data.Instance.events.OnJoystickUp += OnJoystickUp;
		Data.Instance.events.OnJoystickLeft += OnJoystickLeft;
		Data.Instance.events.OnJoystickRight += OnJoystickRight;
		Invoke ("SetCanInteract", 1);
		Invoke ("TimeOver", 40);
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
	//	Data.Instance.events.OnJoystickDown -= OnJoystickDown;
	//	Data.Instance.events.OnJoystickUp -= OnJoystickUp;
		Data.Instance.events.OnJoystickLeft -= OnJoystickLeft;
		Data.Instance.events.OnJoystickRight -= OnJoystickRight;
	}
	void OnJoystickClick()
	{
		if (!canInteract)
			return;
		
		canInteract = false;
		computerUI.SetActive (false);
		diskette.SetOn ();
		Invoke ("Delayed", 4f);
		camAnimnation.Play ("levelSelectorCamera");
	}
	void Delayed()
	{
		Data.Instance.videogamesData.actualID = videgameID;
		Data.Instance.missions.ActivateFirstGameByVideogame (videogameData.id);
		Data.Instance.LoadLevel ("Game");
	}
	void OnJoystickUp()
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
	void OnJoystickDown()
	{
		if (!canInteract)
			return;
		if(videgameID>0)
			videgameID--;
		else
			return;
		
		SetSelected ();	
	}
	void OnJoystickLeft()
	{		
		OnJoystickUp ();
	}
	void OnJoystickRight()
	{
		OnJoystickDown ();
	}
	void SetSelected()
	{
		videogameData = Data.Instance.videogamesData.all [videgameID];
		diskette.Init (videogameData);
		videogameUI.Change ();
	}
	public void OnJoystickBack()
	{
		Data.Instance.LoadLevel("MainMenu");
	}
}
