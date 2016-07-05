using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ArcadeGUI : MonoBehaviour {

   // public int score;
    public bool ended;
    public MultiplayerUIStatus[] multiplayerUI;
    public List<int> multiplayerUI_Y;
    private CharactersManager characterManager;
    public GameObject gameOver;

	void Start () {
        gameOver.SetActive(false);
        characterManager = Game.Instance.GetComponent<CharactersManager>();
        ended = false;
        Data.Instance.events.OnScoreOn += OnScoreOn;
        Data.Instance.events.OnGameOver += OnGameOver;
        Data.Instance.events.OnAddNewPlayer += OnAddNewPlayer;
        Data.Instance.events.OnAvatarDie += OnAvatarDie;
        StartMultiplayerStatus();
	}
    void Update()
    {
        if (ended) return;
        if (Input.GetKeyDown(KeyCode.Alpha1) && CanRevive(0))
        {
            if (!characterManager.existsPlayer(0))
                characterManager.addNewCharacter(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && CanRevive(1))
        {
            if (!characterManager.existsPlayer(1))
                characterManager.addNewCharacter(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && CanRevive(2))
        {
            if (!characterManager.existsPlayer(2))
                characterManager.addNewCharacter(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && CanRevive(3))
        {
            if (!characterManager.existsPlayer(3))
                characterManager.addNewCharacter(3);
        }
    }
    void OnDestroy()
    {
        Data.Instance.events.OnScoreOn -= OnScoreOn;
        Data.Instance.events.OnGameOver -= OnGameOver;
        Data.Instance.events.OnAddNewPlayer -= OnAddNewPlayer;
        Data.Instance.events.OnAvatarDie -= OnAvatarDie;
    }
    void OnGameOver()
    {
        Data.Instance.multiplayerData.distance = Game.Instance.GetComponent<CharactersManager>().distance;

        gameOver.SetActive(true);
        gameOver.GetComponent<Animation>().Play("gameOver");
        ended = true;
        Data.Instance.scoreForArcade = 0;
        Invoke("Reset", 2);

        foreach (MultiplayerUIStatus muis in multiplayerUI)
        {
            switch (muis.id)
            {
                case 0: Data.Instance.multiplayerData.score_player1 = muis.score; break;
                case 1: Data.Instance.multiplayerData.score_player2 = muis.score; break;
                case 2: Data.Instance.multiplayerData.score_player3 = muis.score; break;
                case 3: Data.Instance.multiplayerData.score_player4 = muis.score; break;
            }
            muis.gameObject.SetActive(false);
        }
    }
    void Reset()
    {
        Data.Instance.events.OnResetLevel();
        Data.Instance.LoadLevel("SummaryMultiplayer");        
    }
    void OnScoreOn(int playerID, Vector3 pos, int total)
    {
        SetScoreToUI(playerID, total);
	}
    void StartMultiplayerStatus()
    {
        int id = 0;        
        foreach (MultiplayerUIStatus muis in multiplayerUI)
        {
            multiplayerUI_Y.Add((int)muis.transform.localPosition.y);
            bool active = false;
            muis.Init(id, Game.Instance.GetComponent<CharactersManager>().colors[id], active);
            id++;
        }
        Loop();
    }
    void Loop()
    {

        //Re-order avatars for score:
        bool reorder = false;
        for (int a = 0; a<multiplayerUI.Length; a++)
        {
            if (a > 0)
            {
                if (multiplayerUI[a].score > multiplayerUI[a - 1].score)
                {
                    reorder = true;
                    MultiplayerUIStatus m_loser = multiplayerUI[a - 1];
                    multiplayerUI[a - 1] = multiplayerUI[a];
                    multiplayerUI[a] = m_loser;
                }
            }
        }
        if (reorder)
        {
            List<int> positions = new List<int>();
            for (int a = 0; a < multiplayerUI.Length; a++)
            {
                if (multiplayerUI[a].score >0)
                positions.Add(multiplayerUI[a].id);
            }
            Data.Instance.events.OnReorderAvatarsByPosition(positions);

            for (int a = 0; a < multiplayerUI.Length; a++)
            {
                if (multiplayerUI[a].transform.localPosition.y != multiplayerUI_Y[a])
                    multiplayerUI[a].MoveTo(multiplayerUI_Y[a]);
            }
        }
        /////////////////////////////
        Invoke("Loop", 1f);
    }
    private MultiplayerUIStatus GetPlayerUI(int playerID)
    {
        for (int a = 0; a < multiplayerUI.Length; a++)
        {
            if (multiplayerUI[a].id == playerID)
            {
                return multiplayerUI[a];
            }
        }
        return null;
    }
    void SetScoreToUI(int id, int score)
    {
        GetPlayerUI(id).AddScore(score);
    }
    void OnAddNewPlayer(int id)
    {
        GetPlayerUI(id).Active();
    }
    void OnAvatarDie(CharacterBehavior cb)
    {
        GetPlayerUI(cb.player.id).Dead();
    }
    private bool CanRevive(int playerID)
    {
        if (GetPlayerUI(playerID).state == MultiplayerUIStatus.states.DEAD)
            return false;
        return true;
    }
}
