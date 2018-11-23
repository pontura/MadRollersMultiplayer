using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoystickWeaponUI : MonoBehaviour {

	public Image image;
	public GameObject panel;
	int playerID;

	void Start () {
		playerID = GetComponent<JoystickPlayer> ().playerID;
		Reset ();
		Data.Instance.events.OnChangeWeapon += OnChangeWeapon;
		image.color = Data.Instance.multiplayerData.colors [playerID];
	}
	void OnDestroy () {
		Data.Instance.events.OnChangeWeapon -= OnChangeWeapon;
	}

	void OnChangeWeapon (int _playerID, Weapon.types type) {
		
		if (playerID != _playerID)
			return;
		
		panel.SetActive (true);

		print ("carga: " + "bullets/" + type.ToString ());

		Sprite texture = Resources.Load<Sprite>("bullets/" + type.ToString());
		image.sprite = texture;

		Invoke ("Reset", 1);

	}
	public void Reset()
	{
		panel.SetActive (false);
	}
}
