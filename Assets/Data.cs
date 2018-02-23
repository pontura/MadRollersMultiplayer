using UnityEngine;
using System.Collections;

public class Data : MonoBehaviour {

	public bool RESET;
	public bool turnOffSounds;
    public bool musicOn = true;
    public bool switchPlayerInputs;
    public int competitionID = 1;
    public bool isArcade;
    public bool isArcadeMultiplayer;

    public int levelUnlockedID = 0;
    public int totalReplays = 3;
    public int replays = 0;
    public float volume;
    public int scoreForArcade;

    public int WebcamID;

    [HideInInspector]
    public UserData userData;
    [HideInInspector]
    public Events events;
    [HideInInspector]
    public ObjectPool sceneObjectsPool;
    [HideInInspector]
    public Missions missions;
    [HideInInspector]
    public Competitions competitions;
    [HideInInspector]
    public MultiplayerData multiplayerData;
	[HideInInspector]
	public VideogamesData videogamesData;

    static Data mInstance = null;

    public modes mode;

    public VoicesManager voicesManager;

    public bool DEBUG;
    public int FORCE_LOCAL_SCORE;

    public PlayModes playMode;
    public enum PlayModes
    {
        STORY,
        COMPETITION
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

        competitions.Init();
        if(userData)
            userData.Init();
        

        GetComponent<MusicManager>().Init();
        GetComponent<Tracker>().Init();
        GetComponent<Missions>().Init();
        GetComponent<CurvedWorldManager>().Init();

        //GetComponent<DataController>().Init();
        levelUnlockedID = PlayerPrefs.GetInt("levelUnlocked");

        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            DEBUG = false;
            mode = modes.ACCELEROMETER;            
        }
        voicesManager.Init();

       // if (Application.isWebPlayer)
        //    Application.ExternalCall("OnUnityReady");

        events.SetVolume += SetVolume;
	}
    void SetVolume(float vol)
    {
        volume = vol;
    }
	public void setMission(int num)
	{
        //print("MODE: " + playMode + " Set NEW mission " + num + "   levelUnlockedID: " + levelUnlockedID);
        replays = 0;

		missions.MissionActiveID = num;

        if (playMode == PlayModes.COMPETITION)
        {
            SocialEvents.OnMissionReady(num);
        }
        else
        {
            if (num > levelUnlockedID)
            {
                levelUnlockedID = num;
                PlayerPrefs.SetInt("levelUnlocked", num);
            }
        }
	}
    public void resetProgress()
    {
        PlayerPrefs.DeleteAll();
        levelUnlockedID = 0;

        if (isArcade) return;
        SocialEvents.OnCompetitionHiscore(1, 0, false);
        userData.resetProgress();
        Social.Instance.hiscores.Reset();  
    }
    public void LoadLevel(string levelName)
    {
        GetComponent<Fade>().LoadLevel(levelName, 0.6f, Color.black);
    }
}
