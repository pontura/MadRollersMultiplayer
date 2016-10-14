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

    public GameObject players;

    public int sec = 10;
    bool playing;
    public int totalPlayers = 0;
    private bool done;

	void Start () {
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
	}
    void Update()
    {
        if (done) return;
        if ((InputManager.getFire(0) || InputManager.getJump(0)))
        {
            Clicked(0);
        }
        if ((InputManager.getFire(1) || InputManager.getJump(1)))
        {
            Clicked(1);
        }
        if ((InputManager.getFire(2) || InputManager.getJump(2)))
        {
            Clicked(2);
        }
        if ((InputManager.getFire(3) || InputManager.getJump(3)))
        {
            Clicked(3);
        }
    }
    bool anyActive = false;
    void Clicked(int playerID)
    {
        totalPlayers = 0;
        Data.Instance.events.OnSoundFX("FXCheer", playerID);

        playerMainMenuUI[playerID].Toogle();
        mainMenuCharacterActor[playerID].SetState(playerID, playerMainMenuUI[playerID].isActive);

        GetTotalPlayers();        

        if (!anyActive)
        {
            playing = false;
        } else
        if (anyActive && !playing)
        {
            playing = true;
            Loop();
        }
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
        sec--;

        CountDown1.text = "0" + sec;
        CountDown2.text = "0" + sec;

        foreach (Text field in players.GetComponentsInChildren<Text>())
            field.text = totalPlayers + " PLAYERS";

        if (sec < 1)
        {
            GetTotalPlayers();
            if (totalPlayers > 0)
            {
                done = true;
                Data.Instance.LoadLevel("GameMultiplayer");
                return;
            }
            else sec = 1;
        }
        Invoke("Loop", 0.3f);
    }
}
