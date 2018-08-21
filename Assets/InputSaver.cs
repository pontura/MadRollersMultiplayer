using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSaver : MonoBehaviour {

	public List<InputSaverData> recordingList;

	void Start () {
		//Data.Instance.events.OnAvatarShoot += OnAvatarShoot;
		//Data.Instance.events.OnAvatarJump += OnAvatarJump;
		//Data.Instance.events.OnGameOver += OnGameOver;
	}
	public void MoveInX(float value, Vector3 pos)
	{
		InputSaverData isd = new InputSaverData ();
		isd.direction = value;
		isd.posX = pos.x;
		AddToRecordingList (isd);
	}
	void OnAvatarShoot(int value)
	{
		InputSaverData isd = new InputSaverData ();
		isd.shoot = true;
		AddToRecordingList (isd);
	}
	void OnAvatarJump(int id)
	{
		InputSaverData isd = new InputSaverData ();
		isd.jump = true;
		AddToRecordingList (isd);
	}
	void AddToRecordingList(InputSaverData isd)
	{
		isd.distance = Game.Instance.level.charactersManager.distance;
		recordingList.Add (isd);
	}
	void OnGameOver()
	{
		GetComponent<InputSavedAutomaticPlay> ().Reset();
		GetComponent<InputSavedAutomaticPlay> ().SaveNewList( recordingList );
		recordingList.Clear ();
	}
}
