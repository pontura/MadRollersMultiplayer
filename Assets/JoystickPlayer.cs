using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoystickPlayer : MonoBehaviour {

	public int playerID;
	public GameObject joystick;
	public GameObject button1;
	public GameObject button2;
	public GameObject insertCoin;
	public GameObject dead;
	public Image deadFill;

	public states state;

	public enum states
	{
		INSERT_COIN,
		PLAYING,
		DEAD
	}
	void Start()
	{
		RefreshStates ();
		deadFill.color = Data.Instance.multiplayerData.colors [playerID];
	}
	public void RefreshStates() {	
		if (Data.Instance.playMode == Data.PlayModes.VERSUS) {

			dead.SetActive (false);
			insertCoin.SetActive (false);
			gameObject.SetActive (false);

			if (Data.Instance.multiplayerData.player1 && playerID == 0)
				gameObject.SetActive (true);
			if (Data.Instance.multiplayerData.player2 && playerID == 1)
				gameObject.SetActive (true);
			if (Data.Instance.multiplayerData.player3 && playerID == 2)
				gameObject.SetActive (true);
			if (Data.Instance.multiplayerData.player4 && playerID == 3)
				gameObject.SetActive (true);	
		} else {
			if (playerID == 0) {
				if(Data.Instance.multiplayerData.player1)
					SetState (states.PLAYING);
				else
					SetState (states.INSERT_COIN);
			} else if (playerID == 1) {
				if(Data.Instance.multiplayerData.player2)
					SetState (states.PLAYING);
				else
					SetState (states.INSERT_COIN);
			} else if (playerID == 2) {
				if(Data.Instance.multiplayerData.player3)
					SetState (states.PLAYING);
				else
					SetState (states.INSERT_COIN);
			} else if (playerID == 3) {
				if(Data.Instance.multiplayerData.player4)
					SetState (states.PLAYING);
				else
					SetState (states.INSERT_COIN);
			}
		}
	}
	public void SetState(states _state)
	{
		this.state = _state;
		switch (state) {
		case states.INSERT_COIN:
			dead.SetActive (false);
			insertCoin.SetActive (true);
			break;
		case states.DEAD:
			fillAmount = 0;
			dead.SetActive (true);
			insertCoin.SetActive (false);
			break;
		case states.PLAYING:
			dead.SetActive (false);
			insertCoin.SetActive (false);
			break;
		}
	}

	void Update () {
		if (InputManager.getFire (playerID)) 
			PressButton(button1);
		else
			ResetButton (button1);
		
		if (InputManager.getJump(playerID))
			PressButton(button2);
		else
			ResetButton (button2);

		float h = InputManager.getHorizontal (playerID);
		SetHorizontal (h);

		if (state == states.DEAD)
			FillDead ();
		
	}
	void SetHorizontal(float value)
	{
		Vector3 pos = joystick.transform.localEulerAngles;
		pos.z = value*-15;
		joystick.transform.localEulerAngles = pos;
	}
	void PressButton(GameObject button)
	{
		if (button.transform.localPosition.y == -1.5f)
			return;
		Vector2 pos = button.transform.localPosition;
		pos.y = -1.5f;
		button.transform.localPosition = pos;

	}
	void ResetButton(GameObject button)
	{
		if (button.transform.localPosition.y == 0)
			return;

		Vector2 pos = button.transform.localPosition;
		pos.y = 0;
		button.transform.localPosition = pos;		
	}
	float fillAmount;
	void FillDead()
	{
		deadFill.fillAmount = fillAmount;
		fillAmount += Time.deltaTime / 5;
		if (fillAmount > 1)
			SetState (states.INSERT_COIN);
	}
}
