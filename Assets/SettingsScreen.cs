using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SettingsScreen : MonoBehaviour {

	public Toggle switchPlayers;
	public Toggle musicToggle;
	public Toggle soundFXToggle;
	public Toggle voicesFXToggle;
	public Toggle madRollersToggle;

	void Start () {
		Cursor.visible = true;
		Invoke("Next", 1);
	}
	public void Done()
	{
		Data.Instance.musicOn = musicToggle.isOn;
		Data.Instance.soundsFXOn = soundFXToggle.isOn;
		Data.Instance.madRollersSoundsOn = madRollersToggle.isOn;
		Data.Instance.voicesOn = voicesFXToggle.isOn;
		Data.Instance.switchPlayerInputs = switchPlayers.isOn;

		Data.Instance.events.OnMusicStatus (Data.Instance.musicOn);
		Data.Instance.events.OnSFXStatus (Data.Instance.soundsFXOn);
		Data.Instance.events.OnMadRollersSFXStatus (Data.Instance.madRollersSoundsOn);
		Data.Instance.events.OnVoicesStatus (Data.Instance.voicesOn);

		Cursor.visible = false;
		Data.Instance.LoadLevel("MainMenu");
	}

}
