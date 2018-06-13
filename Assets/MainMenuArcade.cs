using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuArcade : MonoBehaviour {

    public PlayerMainMenuUI[] playerMainMenuUI;
    public MainMenuCharacterActor[] mainMenuCharacterActor;
    private MultiplayerData multiplayerData;

    public AudioClip countdown_clip;
    private AudioSource audioSource;

    public Text CountDown1;
    public Text CountDown2;

	public Text[] missionFields;

    public GameObject players;
    public GameObject winnersText;

    public int sec = 10;
    bool playing;
    public int totalPlayers = 0;
    private bool done;

    public MeshRenderer winnersPicture;
  //  public Light lightInScene;
    public Material[] backgrounds;
    public MeshRenderer backgruond;

	void Start () {
		string desc = Data.Instance.missions.GetMissionActive ().description;
		foreach (Text t in missionFields) {
			t.text = desc;
		}
        Data.Instance.events.OnInterfacesStart();
        audioSource = GetComponent<AudioSource>();
        multiplayerData = Data.Instance.multiplayerData;
        CountDown1.text = "";
        CountDown2.text = "";
        int id = 0;
        foreach (PlayerMainMenuUI pm in playerMainMenuUI)
        {
            pm.id = id;
            id++;
            pm.Init();
        }
		if (Data.Instance.playMode == Data.PlayModes.COMPETITION ) {
			LoopWinners ();
			SetFields (0);
		}
		Invoke ("TimeOver", 15);
	}
	void TimeOver()
	{
		Data.Instance.LoadLevel("MainMenu");
	}
    void LoopBG()
    {
       // backgruond.material = backgrounds[Random.Range(0, backgrounds.Length-1)];
       // Invoke("LoopBG", Random.Range(10, 40) / 10);
    }
    int actualWinner;
    void LoopWinners()
    {
        float timeToLoop = 2;
        if (actualWinner > 0)
            timeToLoop = 0.7f;
        
        winnersPicture.material.mainTexture = Data.Instance.GetComponent<ArcadeRanking>().all[actualWinner].texture;
        actualWinner++;
        if (actualWinner >= Data.Instance.GetComponent<ArcadeRanking>().all.Count)
            actualWinner = 0;
        Invoke("LoopWinners", timeToLoop);
        
    }
    void SetFields(int puesto)
    {
        int hiscore = Data.Instance.GetComponent<ArcadeRanking>().all[puesto].score;
        string actualCompetition = Data.Instance.GetComponent<MultiplayerCompetitionManager>().actualCompetition;
        foreach (Text field in winnersText.GetComponentsInChildren<Text>())
        {
            if (puesto == 0)
                field.text = "PUNTERx/S (" + hiscore + " PUNTOS) - CAMPEONATO " + actualCompetition + " -";
           // else
              //  field.text = "PUESTO " + (int)(puesto + 1);
        }
    }
    int n = 0;
    void Update()
    {
        n++;
        if (n > 5)
        {
         //   lightInScene.intensity = (float)Random.Range(80, 150) / 100;
            n = 0;
        }
        if (done) return;
		if ((InputManager.getFireDown(0) || InputManager.getJump(0)))
        {
            Clicked(0);
        }
		if ((InputManager.getFireDown(1) || InputManager.getJump(1)))
        {
            Clicked(1);
        }
		if ((InputManager.getFireDown(2) || InputManager.getJump(2)))
        {
            Clicked(2);
        }
		if ((InputManager.getFireDown(3) || InputManager.getJump(3)))
        {
            Clicked(3);
        }
    }
    bool anyActive = false;
    void Clicked(int playerID)
    {
		if (playerMainMenuUI [playerID].isActive)
			return;
		
        totalPlayers = 0;
        Data.Instance.events.OnSoundFX("coin", playerID);

        playerMainMenuUI[playerID].Toogle();
        mainMenuCharacterActor[playerID].SetState(playerID, playerMainMenuUI[playerID].isActive);

        GetTotalPlayers();        

//        if (!anyActive)
//        {
//            playing = false;
//        } else
//        if (anyActive && !playing)
//        {
            playing = true;
            Loop();
      //  }
    }
    void GetTotalPlayers()
    {
        foreach (PlayerMainMenuUI pm in playerMainMenuUI)
        {
            if (pm.id == 0) { multiplayerData.player1 = pm.isActive; }
            if (pm.id == 1) { multiplayerData.player2 = pm.isActive; }
            if (pm.id == 2) { multiplayerData.player3 = pm.isActive; }
            if (pm.id == 3) { multiplayerData.player4 = pm.isActive; }

            if (pm.isActive)
            {
                anyActive = true;
                totalPlayers++;
            }
        }
    }
    void Loop()
    {
        if (!playing) return;
		if (Data.Instance.playMode == Data.PlayModes.VERSUS)
		if (
			(Data.Instance.multiplayerData.player1 || Data.Instance.multiplayerData.player2)
			&&
			(Data.Instance.multiplayerData.player3 || Data.Instance.multiplayerData.player4)) {
			//sigue
		} else
			return;
		
        sec--;

        CountDown1.text = "0" + sec;
        CountDown2.text = "0" + sec;

        foreach (Text field in players.GetComponentsInChildren<Text>())
            field.text = totalPlayers + " PLAYERS";

        if (sec < 1)
        {
            GetTotalPlayers();
			if (
				(Data.Instance.playMode== Data.PlayModes.COMPETITION && totalPlayers > 0)
				||  (Data.Instance.playMode== Data.PlayModes.VERSUS && totalPlayers > 1)
			)
            {
                done = true;
				if (Data.Instance.playMode == Data.PlayModes.COMPETITION ) {
					Data.Instance.LoadLevel ("Game");
				} else {
					Data.Instance.LoadLevel ("GameVersus");
				}
                return;
            }
            else sec = 1;
        }
        Invoke("Loop", 0.3f);
    }
}
