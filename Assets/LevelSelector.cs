using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LevelSelector : MonoBehaviour {

	public int missionActiveID;
	public int videogameActiveID;

    public MissionButton uiButton;
	public Transform container;
    [SerializeField]

	public float separation_in_x = 115;
	private Data data;

	private Missions missions;
	private int missionID;
    private float StartScrollPosition;
    private MissionButton lastButton;
    public bool showJoystickMenu;
	int separation = 50;
	public GameObject cam;
	float lastClickedTime;
	List<MissionButton> all;
	public MissionButton lastButtonSelected;

	public List<MissionsByVideogame> allMissionsByVideogame;

	[Serializable]
	public class MissionsByVideogame
	{
		public List<Mission> missions;
	}

	VideogamesUIManager videogameUI;

	void Start()
	{
		videogameActiveID = 0;
		missionActiveID = 0;
		if (Data.Instance.playMode == Data.PlayModes.STORY)
			InitStoryMode ();
		else {
			separation *= 2;
			InitComtetitionMode ();
		}
		
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
		if (lastButtonSelected == null)
			return;
		//cam.transform.localPosition = Vector3.Lerp (cam.transform.localPosition, new Vector3(0, 0, missionActiveID * separation), 0.1f);
		MissionButton missionButton = lastButtonSelected;
		float _x = 0;
		float _z = 0;
		if (Data.Instance.playMode == Data.PlayModes.STORY) {
			_x = videogameActiveID * separation_in_x;
			_z = missionButton.id_in_videogame * separation;
		} else {
			_z = videogameActiveID * separation;
		}
		cam.transform.localPosition = Vector3.Lerp (cam.transform.localPosition, new Vector3(_x, 0, _z), Time.deltaTime*10);
	}
	void OnJoystickClick()
	{
		if (lastButtonSelected == null || lastButtonSelected.isLocked)
			return;
		Data.Instance.videogamesData.ChangeID (videogameActiveID);
		lastButtonSelected.OnClick ();
		Invoke ("Delayed", 0.3f);
	}
	void Delayed()
	{
		data.missions.MissionActiveID =lastButtonSelected.id;

		if (
			(Data.Instance.playMode == Data.PlayModes.STORY
			)) {
			Data.Instance.LoadLevel("Game");
			Data.Instance.isReplay = true;
		}	else 
			Data.Instance.LoadLevel("MainMenuArcade");
	}
	void OnJoystickUp()
	{
		if (Data.Instance.playMode == Data.PlayModes.COMPETITION) {
			OnJoystickLeft ();
			return;
		} else
		if(missionActiveID<allMissionsByVideogame[videogameActiveID].missions.Count-1)
			missionActiveID++;
		SetSelected ();
	}
	void OnJoystickDown()
	{
		if (Data.Instance.playMode == Data.PlayModes.COMPETITION) {
			OnJoystickRight ();
			return;
		} else
		if(missionActiveID>0)
			missionActiveID--;
		SetSelected ();
	}
	void OnJoystickLeft()
	{		
		if(videogameActiveID == (Data.Instance.videogamesData.all.Length-1))
			return;
		missionActiveID = 0;
		videogameActiveID++;
		videogameUI.Right ();
	}
	void OnJoystickRight()
	{
		if(videogameActiveID==0)
			return;
		missionActiveID = 0;
		videogameActiveID--;
		videogameUI.Left ();
	}

	void InitComtetitionMode()
	{
		Data.Instance.events.OnInterfacesStart();

		data = Data.Instance;		
		missions = data.missions;

		int videogameID = 0;
		int lastVideoGameID = -1;

		all = new List<MissionButton> ();
		int id_in_videogame = 0;
		foreach (Mission mission in missions.missions) {



			if (lastVideoGameID != mission.videoGameID) {
				MissionButton button = Instantiate (uiButton) as MissionButton;

				button.Init(mission, missionID);
				lastVideoGameID = mission.videoGameID;
				id_in_videogame = 0;

				MissionsByVideogame mbv = new MissionsByVideogame ();
				allMissionsByVideogame.Add (mbv);
				mbv.missions = new List<Mission> ();

				button.id_in_videogame = id_in_videogame;
				lastButton = button;
				button.videoGameID = mission.videoGameID;

				if (videogameID==0 && id_in_videogame > data.levelUnlocked_level_1 && !Data.Instance.DEBUG)
					button.disableButton ();
				else if (videogameID==1 && id_in_videogame > data.levelUnlocked_level_2 && !Data.Instance.DEBUG)
					button.disableButton ();


				videogameID = mission.videoGameID;

				missionID++;

				all.Add (button);
				allMissionsByVideogame [videogameID].missions.Add (mission);

			} else
				id_in_videogame++;
		}
		all.Sort(GetIdByVideogame);
		//all.Reverse ();

		int lasAddedVideoGameButtonID = -1;
		foreach (MissionButton mission in all) {
			if (mission.videoGameID != lasAddedVideoGameButtonID) {
				lasAddedVideoGameButtonID = mission.videoGameID;
				mission.transform.SetParent (container);
				mission.transform.localScale = new Vector3 (1, 1, 1);
				mission.transform.localPosition = new Vector3 (0, 0, mission.videoGameID * separation);
			}
		}
		videogameUI = GetComponent<VideogamesUIManager> ();
		videogameUI.Init (0);
		all.Reverse ();
		SetSelected ();
	}




	void InitStoryMode () {

		Data.Instance.events.OnInterfacesStart();

		data = Data.Instance;		
		missions = data.missions;

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

				MissionsByVideogame mbv = new MissionsByVideogame ();
				allMissionsByVideogame.Add (mbv);
				mbv.missions = new List<Mission> ();

			} else
				id_in_videogame++;



			button.id_in_videogame = id_in_videogame;
			lastButton = button;
			button.videoGameID = mission.videoGameID;

			if (videogameID==0 && id_in_videogame > data.levelUnlocked_level_1 && !Data.Instance.DEBUG)
				button.disableButton ();
			else if (videogameID==1 && id_in_videogame > data.levelUnlocked_level_2 && !Data.Instance.DEBUG)
				button.disableButton ();


			videogameID = mission.videoGameID;

			missionID++;

			all.Add (button);
			allMissionsByVideogame [videogameID].missions.Add (mission);

		}
		all.Sort(GetIdByVideogame);
		all.Reverse ();

		foreach (MissionButton mission in all) {
			mission.transform.SetParent (container);
			mission.transform.localScale = new Vector3(1,1,1);
			mission.transform.localPosition = new Vector3 (mission.videoGameID * separation_in_x, 0, mission.id_in_videogame * separation);
		}

		videogameUI = GetComponent<VideogamesUIManager> ();
		videogameUI.Init (0);

		all.Reverse ();
		SetSelected ();


	}
	int GetIdByVideogame(MissionButton button1, MissionButton button2)
	{
		return button1.id_in_videogame.CompareTo (button2.id_in_videogame);
	}

	void SetSelected()
	{
		if(lastButtonSelected!=null)
			lastButtonSelected.SetOn (false);
		Mission missionActive = allMissionsByVideogame [videogameActiveID].missions [missionActiveID];
		lastButtonSelected = GetButtonByMission(missionActive);
		lastButtonSelected.SetOn (true);
		videogameUI.Select (lastButtonSelected.mission.videoGameID);
	}
	public void OnJoystickBack()
    {
		
        Data.Instance.LoadLevel("MainMenu");
    }
	public void SelectFirstLevelOf(int videoGameID)
	{
		missionActiveID = 0;
		SetSelected ();
	}
	MissionButton GetButtonByMission(Mission mission)
	{
		foreach (MissionButton missionButton in all) {
			if (missionButton.mission == mission)
				return missionButton;			
		}
		return null;
	}
}
