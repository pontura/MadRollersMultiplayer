﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsModeScreen : MonoBehaviour {

	public void InitStoryMode () {
		Data.Instance.playMode = Data.PlayModes.STORYMODE;
		Go ();
	}

	public void InitPartyMode () {
		Data.Instance.playMode = Data.PlayModes.PARTYMODE;
		Go ();
	}
	void Go()
	{
		Data.Instance.events.OnMusicStatus (true);
		Data.Instance.events.OnSFXStatus (true);
		Data.Instance.events.OnMadRollersSFXStatus (true);
		Data.Instance.events.OnVoicesStatus (true);

		Data.Instance.canContinue = true;
		Data.Instance.musicOn = true;
		Data.Instance.soundsFXOn = true;
		Data.Instance.madRollersSoundsOn = true;
		Data.Instance.voicesOn = true;

		Data.Instance.switchPlayerInputs = false;
		
		Cursor.visible = false;
		Data.Instance.LoadLevel("MainMenu");
	}
}
