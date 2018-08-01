using UnityEngine;
using System.Collections;

public class Data : MonoBehaviour {

	public bool isArcadeMultiplayer;
	public bool DEBUG;

	public bool canContinue;
	public int credits;
	public bool voicesOn;
	public bool soundsFXOn;
	public bool madRollersSoundsOn;
    public bool musicOn;
    public bool switchPlayerInputs;

	public bool isReplay;
	public int totalJoysticks;
	public bool RESET;

    public int competitionID = 1;
    //public bool isArcade;
    

    public int levelUnlocked_level_1 = 0;
	public int levelUnlocked_level_2 = 0;

    public float volume;
    public int scoreForArcade;

	public bool webcamOff;
   // public int WebcamID;

    public UserData userData;
    [HideInInspector]
    public Events events;
    public ObjectPool sceneObjectsPool;
    [HideInInspector]
    public Missions missions;
    [HideInInspector]
    public Competitions competitions;
    [HideInInspector]
    public MultiplayerData multiplayerData;
	[HideInInspector]
	public VideogamesData videogamesData;
	[HideInInspector]
	public InputSaver inputSaver;
	[HideInInspector]
	public InputSavedAutomaticPlay inputSavedAutomaticPlay;

    static Data mInstance = null;

    public modes mode;

    public VoicesManager voicesManager;
	public VersusManager versusManager;

	public LoadingAsset loadingAsset;
   
    public int FORCE_LOCAL_SCORE;

    public PlayModes playMode;
    public enum PlayModes
    {
        STORY,
        COMPETITION,
		VERSUS
	//	GHOSTMODE
    }
    public enum modes
    {
        ACCELEROMETER,
        KEYBOARD,
        JOYSTICK
    }
    public bool hasContinueOnce;

    public static Data Instance
    {
        get
        {
            if (mInstance == null)
            {
                Debug.LogError("Algo llama a DATA antes de inicializarse");
            }
            return mInstance;
        }
    }
	void Awake () {

		Random.seed = 42;

		if (RESET)
			PlayerPrefs.DeleteAll ();
      //  Cursor.visible = false;

        if (FORCE_LOCAL_SCORE > 0 )
            PlayerPrefs.SetInt("scoreLevel_1", FORCE_LOCAL_SCORE);

        mInstance = this;
		DontDestroyOnLoad(this);
        

		//setAvatarUpgrades();
       // levelUnlockedID = PlayerPrefs.GetInt("levelUnlocked_0");
        events = GetComponent<Events>();
        missions = GetComponent<Missions>();
        competitions = GetComponent<Competitions>();
        multiplayerData = GetComponent<MultiplayerData>();
		videogamesData = GetComponent<VideogamesData> ();
		inputSaver = GetComponent<InputSaver> ();
		inputSavedAutomaticPlay = GetComponent<InputSavedAutomaticPlay> ();
		versusManager = GetComponent<VersusManager> ();

		if (totalJoysticks > 0)
			multiplayerData.player1 = true;
		if (totalJoysticks > 1)
			multiplayerData.player2 = true;
		if (totalJoysticks > 2)
			multiplayerData.player3 = true;
		if (totalJoysticks > 3)
			multiplayerData.player4 = true;

       // competitions.Init();
        if(userData)
            userData.Init();
		
        GetComponent<Tracker>().Init();
        GetComponent<Missions>().Init();
        GetComponent<CurvedWorldManager>().Init();

       // GetComponent<DataController>().Init();
		//levelUnlocked_level_1 = PlayerPrefs.GetInt("levelUnlocked_level_1");
		//levelUnlocked_level_2 = PlayerPrefs.GetInt("levelUnlocked_level_2");

		levelUnlocked_level_1 = 100;
		levelUnlocked_level_2 = 100;

        voicesManager.Init();

        events.SetVolume += SetVolume;
	}
	void Start()
	{
		GetComponent<PhotosManager>().LoadPhotos();
	}
    void SetVolume(float vol)
    {
        volume = vol;
    }
	public void setMission(int num)
	{

		missions.MissionActiveID = num;

		int idByVideogame = missions.GetActualMissionByVideogame ();

		if (playMode == PlayModes.COMPETITION)
        {
            SocialEvents.OnMissionReady(num);
        }
        else
        {
			VideogameData vdata = videogamesData.GetActualVideogameData ();
			Mission mission = Data.Instance.missions.missions [num];
			if (vdata.id==0 &&  num > levelUnlocked_level_1)
            {
				levelUnlocked_level_1 = idByVideogame;
				PlayerPrefs.SetInt("levelUnlocked_level_1", idByVideogame);
			} else if (vdata.id==1 &&  num > levelUnlocked_level_2)
			{
				levelUnlocked_level_2 = idByVideogame;
				PlayerPrefs.SetInt("levelUnlocked_level_2", idByVideogame);
			}
        }
	}
    public void resetProgress()
    {
        PlayerPrefs.DeleteAll();
		levelUnlocked_level_1 = 0;
		levelUnlocked_level_2 = 0;

        SocialEvents.OnCompetitionHiscore(1, 0, false);
        userData.resetProgress();
        Social.Instance.hiscores.Reset();  
    }
    public void LoadLevel(string levelName)
    {
		loadingAsset.SetOn (true);
		Data.Instance.events.ForceFrameRate (1);
        GetComponent<Fade>().LoadLevel(levelName, 0.6f, Color.black);
    }
	public void LoadingReady()
	{
		loadingAsset.SetOn (false);
	}
	public void LoadLevelNotFading(string levelName)
	{
		Data.Instance.events.ForceFrameRate (1);
		GetComponent<Fade>().LoadSceneNotFading (levelName);
	}
	public void LoseCredit()
	{
		credits--;
		if (credits == 0)
			credits = 0;
	}
	public void WinCredit()
	{
		credits++;
	}
}
