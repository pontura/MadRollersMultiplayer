﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MadRollersSFX : MonoBehaviour {

	public enum types
	{
		ENGINES,
		JUMP,
		CHEER,
		CRASH,
		TOUCH_GROUND
	}
	public PlayerClips[] playerClips;
	[Serializable]
	public class PlayerClips
	{
		public AudioClip engines;
		public AudioClip jump;
		public AudioClip crash;
		public AudioClip cheer;
		public AudioClip touchGround;
	}

	public AudioSource player1;
	public AudioSource player2;
	public AudioSource player3;
	public AudioSource player4;

	void Start () {
		Data.Instance.events.OnMadRollerFX += OnMadRollerFX;	
		Data.Instance.events.OnGameOver += OnGameOver;
	}

	void OnMadRollerFX(types type, int id)
	{
		print ("OnMadRollerFX " + type + "     id: " + id);
		AudioSource audioSource;
		switch(id)
		{
		case 0: audioSource = player1; break;
		case 1: audioSource = player2; break;
		case 2: audioSource = player3; break;
		default: audioSource = player4; break;			
		}
		AudioClip ac = null;
		switch(type)
		{
		case types.ENGINES:
			ac = playerClips [id].engines;
			audioSource.loop = true; 
			break;
		case types.JUMP: 
			ac = playerClips[id].jump; 
			audioSource.loop = false; 
			break;
		case types.CRASH: 
			ac = playerClips[id].crash; 
			audioSource.loop = false; 
			break;
		case types.CHEER: 
			ac = playerClips[id].cheer; 
			audioSource.loop = false; 
			break;
		case types.TOUCH_GROUND: 
			ac = playerClips[id].touchGround; 
			audioSource.loop = false; 
			break;
		}
		audioSource.clip = ac;
		audioSource.Play ();
	}
	void OnGameOver()
	{
		player1.Stop ();
		player2.Stop ();
		player3.Stop ();
		player4.Stop ();
	}


}
