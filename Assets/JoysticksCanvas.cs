using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoysticksCanvas : MonoBehaviour {

	public List<JoystickPlayer> players;

	void Start()
	{
		Data.Instance.events.OnAddNewPlayer += OnAddNewPlayer;
		Data.Instance.events.OnAvatarDie += OnAvatarDie;
	}
	void OnDestroy()
	{
		Data.Instance.events.OnAddNewPlayer -= OnAddNewPlayer;
		Data.Instance.events.OnAvatarDie -= OnAvatarDie;
	}
	public void RefreshStates() 
	{
		Invoke ("RefreshStatesDelayed", 0.1f);
	}
	public void OnAddNewPlayer(int playerID) 
	{
		players [playerID].SetState (JoystickPlayer.states.PLAYING);
	}
	public void OnAvatarDie(CharacterBehavior cb) 
	{
		if (cb.controls.isAutomata)
			return;
		players [cb.player.id].SetState (JoystickPlayer.states.DEAD);
	}
	public void RefreshStatesDelayed()	
	{
		foreach (JoystickPlayer jp in players)
			jp.RefreshStates ();
	}
	public bool CanRevive(int playerID)
	{
		if (players [playerID].state == JoystickPlayer.states.INSERT_COIN)
			return true;
		return false;
	}

}
