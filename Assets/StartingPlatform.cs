using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingPlatform : SceneObject {

	public GameObject[] platforms;
	public SpriteRenderer logo;

	public int avatarID;

	void OnEnable () {
		Data.Instance.events.OnAvatarShoot += OnAvatarShoot;
		foreach (GameObject go in platforms) {
			Vector3 pos = go.transform.localPosition;
			pos.y = 1f;
			go.transform.localPosition = pos;
		}
		logo.sprite = Data.Instance.videogamesData.GetActualVideogameData ().floppyCover;
	}

	void OnDestroy () {
		Data.Instance.events.OnAvatarShoot -= OnAvatarShoot;
	}

	void OnAvatarShoot(int _avatarID)
	{
		if (avatarID == _avatarID) {
			foreach (GameObject go in platforms) {
				Vector3 pos = go.transform.localPosition;
				pos.y = 0f;
				go.transform.localPosition = pos;
			}
		}
	}
}
