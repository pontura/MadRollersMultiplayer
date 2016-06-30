using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuArcade : MonoBehaviour {

    public PlayerMainMenuUI[] playerMainMenuUI;
    public MainMenuCharacterActor[] mainMenuCharacterActor;
    private MultiplayerData multiplayerData;

    public AudioClip[] select_player_clip;
    public AudioClip countdown_clip;
    private AudioSource audioSource;

    public Text CountDown1;
    public Text CountDown2;
    public int sec = 10;
    bool playing;

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
        if (Input.GetKeyDown(KeyCode.Alpha1)) Clicked(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) Clicked(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) Clicked(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) Clicked(3);
    }
    void Clicked(int playerID)
    {
        audioSource.clip = select_player_clip[ Random.Range(0,select_player_clip.Length) ];
        audioSource.Play();

        playerMainMenuUI[playerID].Toogle();
        mainMenuCharacterActor[playerID].SetState(playerID, playerMainMenuUI[playerID].isActive);

        bool anyActive = false;
        int id;
        foreach (PlayerMainMenuUI pm in playerMainMenuUI)
        {
            if (pm.id == 0) multiplayerData.player1 = pm.isActive;
            if (pm.id == 1) multiplayerData.player2 = pm.isActive;
            if (pm.id == 2) multiplayerData.player3 = pm.isActive;
            if (pm.id == 3) multiplayerData.player4 = pm.isActive;

            if (pm.isActive) anyActive = true;
        }

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
    void Loop()
    {
        if (!playing) return;
        sec--;

        CountDown1.text = "0" + sec;
        CountDown2.text = "0" + sec;
        
        if (sec < 1)
            Data.Instance.LoadLevel("GameMultiplayer");
        else
            Invoke("Loop", 0.3f);
    }
}
