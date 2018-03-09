using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LevelSelector : MonoBehaviour {

    public int levelUnlockedID;
    public MissionButton uiButton;
	public Transform container;
    [SerializeField]

	public float separation_in_x = 115;
	private Data data;
	public int missionActiveID;
	private Missions missions;
	private int missionID;
    private float StartScrollPosition;
    private MissionButton lastButton;
    public bool showJoystickMenu;
	int separation = 50;
	public GameObject cam;
	float lastClickedTime;
	List<MissionButton> all;
	VideogamesUIManager videogameUI;

	void Start()
	{
		Init ();
		Data.Instance.events.OnJoystickBack += OnJoystickBack;
		Data.Instance.events.OnJoystickClick += OnJoystickClick;
		Data.Instance.events.OnJoystickDown += OnJoystickDown;
		Data.Instance.events.OnJoystickUp += OnJoystickUp;
		Data.Instance.events.OnJoystickLeft += OnJoystickLeft;
		Data.Instance.events.OnJoystickRight += OnJoystickRight;
	}
	void OnDestroy()
	{
		Data.Instance.events.OnJoystickBack -= OnJoystickBack;
		Data.Instance.events.OnJoystickClick -= OnJoystickClick;
		Data.Instance.events.OnJoystickDown -= OnJoystickDown;
		Data.Instance.events.OnJoystickUp -= OnJoystickUp;
		Data.Instance.events.OnJoystickLeft -= OnJoystickLeft;
		Data.Instance.events.OnJoystickRight -= OnJoystickRight;
	}
	void Update()
	{
		//cam.transform.localPosition = Vector3.Lerp (cam.transform.localPosition, new Vector3(0, 0, missionActiveID * separation), 0.1f);
		MissionButton missionButton = all[missionActiveID];
		cam.transform.localPosition = Vector3.Lerp (cam.transform.localPosition, new Vector3(missionButton.videoGameID*separation_in_x, 0, missionButton.id_in_videogame * separation), 0.1f);
	}
	void OnJoystickClick()
	{
		if (all [missionActiveID].isLocked)
			return;
		if(lastButtonSelected != null)
			lastButtonSelected.OnClick ();
		Invoke ("Delayed", 0.3f);
	}
	void Delayed()
	{
		data.missions.MissionActiveID = missionActiveID;
		Data.Instance.LoadLevel("Game");
	}
	void OnJoystickUp()
	{
		if(missionActiveID<missions.missions.Length-1)
			missionActiveID++;
		SetSelected ();
	}
	void OnJoystickDown()
	{
		if(missionActiveID>0)
			missionActiveID--;
		SetSelected ();
	}
	void OnJoystickLeft()
	{
		videogameUI.Left ();
	}
	void OnJoystickRight()
	{
		videogameUI.Right ();
	}
	void Init () {

		Data.Instance.events.VoiceFromResources("juega_solo_asi_te_quedaras");
		Data.Instance.events.OnInterfacesStart();

		data = Data.Instance;		
		missions = data.missions;

		missionActiveID = data.levelUnlockedID;

		missionID = 0;
		int videogameID = 0;
		int lastVideoGameID = -1;

		all = new List<MissionButton> ();
		int id_in_videogame = 0;
		foreach (Mission mission in missions.missions) {
			
			MissionButton button = Instantiate (uiButton) as MissionButton;

			button.Init(mission, missionID);

			if (lastVideoGameID != mission.videoGameID) {
				lastVideoGameID = mission.videoGameID;
				id_in_videogame = 0;
			} else
				id_in_videogame++;



			button.id_in_videogame = id_in_videogame;
			lastButton = button;
			button.videoGameID = mission.videoGameID;

			if (missionID > data.levelUnlockedID && !Data.Instance.DEBUG)
				button.disableButton ();
		//	else
			//	videogameID = mission.videoGameID;

			missionID++;

			all.Add (button);
		}
		all.Sort(GetIdByVideogame);
		all.Reverse ();

		foreach (MissionButton mission in all) {

			mission.transform.SetParent (container);
//
//			switch (mission.videoGameID) {
//			case 0:
//				mission.transform.SetParent (videogamesContainer0.transform);
//				break;
//			case 1:
//				mission.transform.SetParent (videogamesContainer1.transform);
//				break;
//			}
//
			mission.transform.localScale = new Vector3(1,1,1);
			mission.transform.localPosition = new Vector3 (mission.videoGameID * separation_in_x, 0, mission.id_in_videogame * separation);
		}
		levelUnlockedID = data.levelUnlockedID;   

		videogameUI = GetComponent<VideogamesUIManager> ();
		videogameUI.Init (videogameID);

		all.Reverse ();
		SetSelected ();


	}
	int GetIdByVideogame(MissionButton button1, MissionButton button2)
	{
		return button1.id_in_videogame.CompareTo (button2.id_in_videogame);
	}
	MissionButton lastButtonSelected;
	void SetSelected()
	{
		if(lastButtonSelected!=null)
			lastButtonSelected.SetOn (false);
		lastButtonSelected = all [missionActiveID];
		lastButtonSelected.SetOn (true);
		videogameUI.Select (lastButtonSelected.mission.videoGameID);
		//missionIcon.SetOn (missions.missions [missionActiveID]);
	}
	public void OnJoystickBack()
    {
        Data.Instance.LoadLevel("MainMenu");
    }
	public void SelectFirstLevelOf(int videoGameID)
	{
		MissionButton firstButton = null;
		foreach (MissionButton missionButton in all) {
			if (missionButton.mission.videoGameID == videoGameID && firstButton == null)
				firstButton = missionButton;
		}
		if (firstButton == null)
			return;
		missionActiveID = firstButton.id;
		lastButtonSelected = firstButton;
		SetSelected ();
	}
}
