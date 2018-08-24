using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingPlatform : SceneObject {

	public GameObject[] platforms;
	public SpriteRenderer logo;
	public Player playerToInstantiate;
	public Transform[] containers;
	public List<int> ids;
	public int avatarID;

	void OnEnable () {
		print ("on enable");
		int id = 0;
		foreach (Transform t in containers) {
			Player newPlayer = Instantiate (playerToInstantiate, Vector3.zero, Quaternion.identity, t);
			newPlayer.transform.localPosition = Vector3.zero;
			newPlayer.isPlaying = false;	
			newPlayer.id = id;
			id++;
		}
		Data.Instance.events.OnAvatarShoot += OnAvatarShoot;
		Data.Instance.events.OnAvatarJump += OnAvatarJump;
//		foreach (GameObject go in platforms) {
//			Vector3 pos = go.transform.localPosition;
//			pos.y = 0.7f;
//			go.transform.localPosition = pos;
//		}
		logo.sprite = Data.Instance.videogamesData.GetActualVideogameData ().floppyCover;
	}

	void OnDestroy () {
		Data.Instance.events.OnAvatarShoot -= OnAvatarShoot;
		Data.Instance.events.OnAvatarJump -= OnAvatarJump;
	}
	void OnAvatarJump(int _avatarID)
	{
		CheckIfStart (_avatarID);
	}
	void OnAvatarShoot(int _avatarID)
	{
		CheckIfStart (_avatarID);
	}
	void CheckIfStart(int _avatarID)
	{		
		foreach (int i in ids) {
			if (i == _avatarID)
				return;
		}
		ids.Add (_avatarID);
		Destroy (containers [_avatarID].gameObject);
//		int id = 0;
//		foreach (GameObject go in platforms) {
//			if (_avatarID == id) {
//				Vector3 pos = go.transform.localPosition;
//				pos.y = -0.4f;
//				go.transform.localPosition = pos;
//			}
//			id++;
//		}
	}
}
