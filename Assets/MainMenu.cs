using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

	public int activeID = 0;
	public List<MainMenuButton> buttons; 
	MainMenuButton activeButton;
	void Start()
	{
		Init ();
		Data.Instance.events.OnJoystickClick += OnJoystickClick;
		Data.Instance.events.OnJoystickDown += OnJoystickDown;
		Data.Instance.events.OnJoystickUp += OnJoystickUp;
		Data.Instance.events.OnJoystickLeft += OnJoystickDown;
		Data.Instance.events.OnJoystickRight += OnJoystickUp;
		Data.Instance.GetComponent<Tracker>().TrackScreen("Main Menu");
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
		else
			Compite ();
	}
	void OnJoystickUp()
	{
//		if (activeID == 1)
//			activeID = 0;
//		else
			activeID=0;
		SetButtons ();
	}
	void OnJoystickDown()
	{
//		if (activeID == 0)
//			activeID = 1;
//		else
			activeID=1;
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
		Data.Instance.playMode = Data.PlayModes.STORY;
		Data.Instance.LoadLevel("LevelSelector");
	}
	void Compite()
	{
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
