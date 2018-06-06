using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ArcadeGUI : MonoBehaviour {

   // public int score;
    public bool ended;
   // public MultiplayerUIStatus[] multiplayerUI;
  //  public List<int> multiplayerUI_Y;
    private CharactersManager characterManager;
    public GameObject singleSignal;
    public GameObject singleSignalTexts;
    public List<int> avatarsThatShoot;
    public states state;
	public GameObject panel;
	public JoysticksCanvas joysticksCanvas;

    public enum states
    {
        INTRO,
        SHOOT_ONE,
        SHOOTS_READY,
        WELLCOME
    }

	void Start () {
//		
//		if (Data.Instance.playMode == Data.PlayModes.STORY)
//			panel.SetActive (false);
		
        singleSignal.SetActive(false);
        characterManager = Game.Instance.GetComponent<CharactersManager>();
        ended = false;
        Data.Instance.events.OnScoreOn += OnScoreOn;
        Data.Instance.events.OnGameOver += OnGameOver;
       
		SetFields ("");
        SetIntroFields();

		if (Data.Instance.isReplay) {
			Invoke ("Delayed", 0.1f);
		}
	}
	void Delayed()
	{
		Data.Instance.events.StartMultiplayerRace ();
		state = states.SHOOTS_READY;
		SetFields ("");
	}
    void SetIntroFields()
    {
		if (state == states.INTRO)
			SetFields (""); //ABRAN PASO!");
		else if (state == states.SHOOT_ONE)
			SetFields ("TODOS DISPAREN!");
		else if (state == states.SHOOTS_READY) {
			if (Data.Instance.playMode == Data.PlayModes.COMPETITION)
				SetFields ("BIEN!...");
			else
				SetFields ("");
		} else if (state == states.WELLCOME) {
			if(Data.Instance.playMode ==Data.PlayModes.COMPETITION )
				SetFields ("...AHORA ROMPAN TODO!");
			else
				SetFields ("");
		}
    }
    void ResetFields()
    {
        if (ended) return;
        if (state == states.WELLCOME)
        {
            SetFields("");return;
        }
        state = states.WELLCOME;
       // SetIntroFields();
        Invoke("ResetFields", 3);

    }
    void Update()
    {
        if (ended) return;
		if ((InputManager.getFireDown(0) || InputManager.getJump(0)) && joysticksCanvas.CanRevive(0))
        {
            if (!characterManager.existsPlayer(0))
                characterManager.addNewCharacter(0);
        }
		else if ((InputManager.getFireDown(1) || InputManager.getJump(1)) && joysticksCanvas.CanRevive(1))
        {
            if (!characterManager.existsPlayer(1))
                characterManager.addNewCharacter(1);
        }
		else if ((InputManager.getFireDown(2) || InputManager.getJump(2)) && joysticksCanvas.CanRevive(2))
        {
            if (!characterManager.existsPlayer(2))
                characterManager.addNewCharacter(2);
        }
		else if ((InputManager.getFireDown(3) || InputManager.getJump(3)) && joysticksCanvas.CanRevive(3))
        {
            if (!characterManager.existsPlayer(3))
                characterManager.addNewCharacter(3);
        }
    }
    void OnDestroy()
    {
        Data.Instance.events.OnScoreOn -= OnScoreOn;
        Data.Instance.events.OnGameOver -= OnGameOver;
       // Data.Instance.events.OnAvatarShoot -= OnAvatarShoot;
    }
    void SetFields(string _text)
    {
        singleSignal.SetActive(true);
        foreach (Text field in singleSignalTexts.GetComponentsInChildren<Text>())
            field.text = _text;
        singleSignal.GetComponent<Animation>().Play("gameOver");
    }
    void OnGameOver()
    {
        print("OnGameOver");
        Data.Instance.multiplayerData.distance = Game.Instance.GetComponent<CharactersManager>().distance;
        
        SetFields("GAME OVER");
        
        ended = true;
        Data.Instance.scoreForArcade = 0;
        Invoke("Reset", 2);

//        foreach (MultiplayerUIStatus muis in multiplayerUI)
//        {
//            switch (muis.id)
//            {
//                case 0: Data.Instance.multiplayerData.score_player1 = muis.score; break;
//                case 1: Data.Instance.multiplayerData.score_player2 = muis.score; break;
//                case 2: Data.Instance.multiplayerData.score_player3 = muis.score; break;
//                case 3: Data.Instance.multiplayerData.score_player4 = muis.score; break;
//            }
//           // muis.gameObject.SetActive(false);
//        }
    }
    void Reset()
    {
		SetFields("");

		return;
		if (Data.Instance.playMode == Data.PlayModes.COMPETITION) {
			Data.Instance.events.OnResetLevel ();
			Data.Instance.LoadLevel ("SummaryMultiplayer");  
		}
    }
    void OnScoreOn(int playerID, Vector3 pos, int total)
    {
		switch (playerID)
		{
		case 0: Data.Instance.multiplayerData.score_player1 += total; break;
		case 1: Data.Instance.multiplayerData.score_player2 += total;  break;
		case 2: Data.Instance.multiplayerData.score_player3+= total;  break;
		case 3: Data.Instance.multiplayerData.score_player4 += total;  break;
		}
		return;

//		if (!isAnActivePlayer(playerID))
//			return;
//        SetScoreToUI(playerID, total);
	}
//    void StartMultiplayerStatus()
//    {
//        int id = 0;        
//        foreach (MultiplayerUIStatus muis in multiplayerUI)
//        {
//            multiplayerUI_Y.Add((int)muis.transform.localPosition.y);
//            bool active = false;
//			muis.Init(id, Data.Instance.GetComponent<MultiplayerData>().colors[id], active);
//            id++;
//        }
//       // Loop();
//    }
    //bool startedSettingPositions;
//    void Loop()
//    {
//
//        //Re-order avatars for score:
//        bool reorder = false;
//        for (int a = 0; a<multiplayerUI.Length; a++)
//        {
//            if (a > 0)
//            {
//                if (multiplayerUI[a].score > multiplayerUI[a - 1].score)
//                {
//                    reorder = true;
//                    MultiplayerUIStatus m_loser = multiplayerUI[a - 1];
//                    multiplayerUI[a - 1] = multiplayerUI[a];
//                    multiplayerUI[a] = m_loser;
//                }
//            }
//        }
//        if (reorder || !startedSettingPositions)
//        {
//            Data.Instance.events.OnSoundFX("FX upgrade004", -1);
//
//            List<int> positions = new List<int>();
//            for (int a = 0; a < multiplayerUI.Length; a++)
//            {
//                if (multiplayerUI[a].score >0)
//                positions.Add(multiplayerUI[a].id);
//            }
//            Data.Instance.events.OnReorderAvatarsByPosition(positions);
//
//            for (int a = 0; a < multiplayerUI.Length; a++)
//            {
//                if (multiplayerUI[a].transform.localPosition.y != multiplayerUI_Y[a])
//                    multiplayerUI[a].MoveTo(multiplayerUI_Y[a]);
//            }
//        }
//        startedSettingPositions = true;
//        Invoke("Loop", 1f);
//    }
//    private MultiplayerUIStatus GetPlayerUI(int playerID)
//    {
//        for (int a = 0; a < multiplayerUI.Length; a++)
//        {
//            if (multiplayerUI[a].id == playerID)
//            {
//                return multiplayerUI[a];
//            }
//        }
//        return null;
//    }
//    void SetScoreToUI(int id, int score)
//    {
//        GetPlayerUI(id).AddScore(score);
//    }

//	bool isAnActivePlayer(int playerID)
//	{
//		if (playerID > 3)
//			return false;
//		return true;
//	}
    
}
