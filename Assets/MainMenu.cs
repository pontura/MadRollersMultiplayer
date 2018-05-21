using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	public int activeID = 0;
	public List<MainMenuButton> buttons; 
	MainMenuButton activeButton;
	public Text playersField;

	void Start()
	{
		Init ();
		Data.Instance.events.OnJoystickClick += OnJoystickClick;
		Data.Instance.events.OnJoystickDown += OnJoystickDown;
		Data.Instance.events.OnJoystickUp += OnJoystickUp;
		Data.Instance.events.OnJoystickLeft += OnJoystickDown;
		Data.Instance.events.OnJoystickRight += OnJoystickUp;
//
//		if (Data.Instance.totalJoysticks == 1)
//			playersField.text = "PLAYER (1)";
//		else 
//			playersField.text = "PLAYERS (" + Data.Instance.totalJoysticks.ToString () + ")";
		
		foreach (MainMenuButton m in buttons)
			m.SetOn (false);
		
		if(Data.Instance.playMode == Data.PlayModes.COMPETITION)
			activeID = 1;
		else
			activeID = 0;
		SetButtons ();
		activeButton.SetOn (true);
	}
	void OnDestroy()
	{
		Data.Instance.events.OnJoystickClick -= OnJoystickClick;
		Data.Instance.events.OnJoystickDown -= OnJoystickDown;
		Data.Instance.events.OnJoystickUp -= OnJoystickUp;
		Data.Instance.events.OnJoystickLeft -= OnJoystickDown;
		Data.Instance.events.OnJoystickRight -= OnJoystickUp;
	}
	void Init () {
		Data.Instance.events.OnInterfacesStart();
		SetButtons ();
	}
	void OnJoystickClick()
	{
		if (activeID == 0)
			MissionsScene ();
		//	Data.Instance.LoadLevel("PlayersSelector");
		else if (activeID == 1) {			
			//Compite ();
			Versus();
		} else if (activeID == 2) {			
			Versus ();
		}

	}
	void OnJoystickUp()
	{
		if (activeID == 0)
			activeID = buttons.Count-1;
		else
			activeID--;
		SetButtons ();
	}
	void OnJoystickDown()
	{
		if (activeID == buttons.Count-1)
			activeID = 0;
		else
			activeID++;
		SetButtons ();
	}
	void SetButtons ()
	{
		if(activeButton != null)
			activeButton.SetOn (false);
		activeButton = buttons [activeID];
		activeButton.SetOn (true);
	}
	void MissionsScene()
	{
		Data.Instance.playMode = Data.PlayModes.COMPETITION;
		//Data.Instance.playMode = Data.PlayModes.STORY;
		Data.Instance.LoadLevel("LevelSelector");
	}
	void Compite()
	{
		Data.Instance.playMode = Data.PlayModes.COMPETITION;
		Data.Instance.LoadLevel("LevelSelector");
//		Data.Instance.playMode = Data.PlayModes.COMPETITION;
//		Data.Instance.LoadLevel("Competitions");
	}
	void Versus()
	{
		Data.Instance.playMode = Data.PlayModes.VERSUS;
		Data.Instance.LoadLevel("LevelSelector");
		//		Data.Instance.playMode = Data.PlayModes.COMPETITION;
		//		Data.Instance.LoadLevel("Competitions");
	}
//	public void Compite()
//	{
//		if (Data.Instance.levelUnlockedID < 4)
//		{
//			Data.Instance.LoadLevel("TrainingSplash");
//		}
//		else
//		{
//			//  Data.Instance.levelUnlockedID = Data.Instance.competitions.GetUnlockedLevel();
//			Data.Instance.playMode = Data.PlayModes.COMPETITION;
//			Data.Instance.LoadLevel("Competitions");
//		}
//	}



}
