﻿using UnityEngine;
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
		Data.Instance.LoseCredit ();
        print("OnGameOver");
        Data.Instance.multiplayerData.distance = Game.Instance.GetComponent<CharactersManager>().distance;
        
		if (Data.Instance.credits > 0) {
			GetComponent<CreditsUI> ().RemoveOne ();
			if (Data.Instance.multiplayerData.GetTotalCharacters () == 1)
				SetFields ("DEAD!");
			else
				SetFields ("ALL DEAD!");

			Invoke("Reset", 2);
		} else {
			SetFields ("GAME OVER");
			Invoke("Reset", 4);
		}
        
        ended = true;
        Data.Instance.scoreForArcade = 0;
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
	}

}
