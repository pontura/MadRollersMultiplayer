using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {

    const string PREFAB_PATH = "Prefabs/Game";
    public GameCamera gameCamera;
	public GameCamera gameCamera2;
    static Game mInstance = null;

	private float pausedSpeed = 0.005f;
	private float pausedMiniumSpeed = 0.05f;
	private bool paused;
	private bool unpaused;

    public MoodManager moodManager;


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
		GetComponent<CompetitionManager> ().Init ();
		GetComponent<CharactersManager>().Init();
		GetComponent<RainManager> ().Init ();
		level.Init();
		gameCamera.Init();
		if (gameCamera2 != null)
			gameCamera2.Init ();

        Data.Instance.events.OnGamePaused += OnGamePaused;
        
        Init();
        Data.Instance.GetComponent<Tracker>().TrackScreen("Game Screen");
        Data.Instance.events.SetSettingsButtonStatus(false);

        Data.Instance.events.OnGameStart();
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
        gameCamera.Init();
		if (gameCamera2 != null)
			gameCamera2.Init ();
        
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
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
    public void GotoLevelSelector()
    {
       // Pause();
        Data.Instance.events.OnResetLevel();
       // Application.LoadLevel("LevelSelector");
        Time.timeScale = 1;
        Data.Instance.LoadLevel("LevelSelector");
    }
    public void GotoMainMenu()
    {
      //  Pause();
        Data.Instance.events.OnResetLevel();
        Time.timeScale = 1;
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
