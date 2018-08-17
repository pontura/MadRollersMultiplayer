using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {

    const string PREFAB_PATH = "Prefabs/Game";
    public GameCamera gameCamera;
    static Game mInstance = null;

	private float pausedSpeed = 0.005f;
	private float pausedMiniumSpeed = 0.05f;
	private bool paused;
	private bool unpaused;

    public MoodManager moodManager;
	public SceneObjectsManager sceneObjectsManager;

    public Level level;

    public static Game Instance
    {
        get
        {
            if (mInstance == null)
            {
                print("Algo llama a Game antes de inicializarse");
            }
            return mInstance;
        }
    }
    void Awake()
    {
        mInstance = this;  

    }
    void Start()
    {
		if (Data.Instance.isReplay) {
			Invoke ("Delayed", 0.2f);
		}
		GetComponent<CompetitionManager> ().Init ();
		GetComponent<CharactersManager>().Init();
		GetComponent<RainManager> ().Init ();
		level.Init();

		if(gameCamera != null)
			gameCamera.Init();

        Data.Instance.events.OnGamePaused += OnGamePaused;
        
        Init();
        Data.Instance.GetComponent<Tracker>().TrackScreen("Game Screen");
        Data.Instance.events.SetSettingsButtonStatus(false);

        //Data.Instance.events.OnGameStart();
    }
	void Delayed()
	{
		Data.Instance.events.OnGameStart();
		Data.Instance.events.StartMultiplayerRace();
	}
    void OnDestroy()
    {
        Data.Instance.events.OnGamePaused -= OnGamePaused;
    }
	private void Init()
	{
		Data.Instance.events.MissionStart(Data.Instance.missions.MissionActiveID);
        Data.Instance.events.OnGamePaused(false);
	}
    public void Revive()
    {
        Data.Instance.events.OnGamePaused(false);

		if(gameCamera != null)
        	gameCamera.Init();
        
        CharacterBehavior cb = level.charactersManager.character;
        
        Vector3 pos = cb.transform.position;
        pos.y = 40;
        pos.x = 0;
        cb.transform.position = pos;

        cb.Revive();
    }
    //pierdo y arranca de ni
    public void ResetLevel()
	{		
		
        Data.Instance.events.OnResetLevel();
        Data.Instance.LoadLevel("Game");
	}
    //IEnumerator  restart()
    //{
    //    yield return new WaitForSeconds(1f);
    //    Debug.Log("_____________Restart");
    //    Application.LoadLevel("Game");
    //}

    public void OnGamePaused(bool paused)
    {
        if (paused)
        {
			Data.Instance.events.ForceFrameRate (0);
        }
        else
        {
			Data.Instance.events.ForceFrameRate (1);
        }
    }
	public void GotoVideogameComplete()
	{
		// Pause();
		Data.Instance.events.OnResetLevel();
		// Application.LoadLevel("LevelSelector");
		Data.Instance.events.ForceFrameRate (1);
		Data.Instance.LoadLevel("VideogameComplete");
	}
    public void GotoLevelSelector()
    {
       // Pause();
        Data.Instance.events.OnResetLevel();
       // Application.LoadLevel("LevelSelector");
		Data.Instance.events.ForceFrameRate (1);
        Data.Instance.LoadLevel("LevelSelector");
    }
	public void GotoMainMenuArcade()
	{
		//  Pause();
		Data.Instance.events.OnResetLevel();
		Data.Instance.events.ForceFrameRate (1);
		Data.Instance.LoadLevel("MainMenuArcade");
	}
    public void GotoMainMenu()
    {
      //  Pause();
        Data.Instance.events.OnResetLevel();
		Data.Instance.events.ForceFrameRate (1);
        Data.Instance.LoadLevel("MainMenu");
    }
    public void GotoContinue()
    {
       // Pause();
        Data.Instance.events.OnResetLevel();
        Time.timeScale = 1;
        Data.Instance.LoadLevel("Continue");
    }
}
