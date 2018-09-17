using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {
	
	private int heightToFall = -5;
	CharacterBehavior cb;
	public int characterPosition;

	void Start()
	{
		cb = GetComponent<CharacterBehavior> ();
		Data.Instance.events.OnReorderAvatarsByPosition += OnReorderAvatarsByPosition;
		Data.Instance.events.StartMultiplayerRace += StartMultiplayerRace;
	}

	void OnDestroy()
	{
		Data.Instance.events.OnReorderAvatarsByPosition -= OnReorderAvatarsByPosition;
		Data.Instance.events.StartMultiplayerRace -= StartMultiplayerRace;
	}
	public void UpdateByController(float rotationY)
	{
		Vector3 goTo = transform.position;

//		if (cb.isOver)
//		{
//			goTo.x = cb.isOver.transform.localPosition.x;
//			goTo.y = cb.isOver.transform.localPosition.y + 1;
//			goTo.z = cb.isOver.transform.localPosition.z+0.2f;
//		}
//		else
//		{

			float _z = cb.player.charactersManager.distance - (characterPosition);
	//		if (controls.isAutomata)
	//			_z -= 2;
	//		if (team_for_versus == 2) {
	//			rotationY *= -1;
	//			_z *= -1;
	//		}
			float speedRotation;
			if (Data.Instance.playMode == Data.PlayModes.VERSUS) {
				speedRotation = 2.2f;
			} else {
				speedRotation = 3;
			}
			goTo.x += (rotationY / speedRotation) * Time.deltaTime;
			goTo.z = _z;
		//}

		if(cb.controls.isAutomata || cb.controls.ControlsEnabled)
			transform.position = Vector3.Lerp(transform.position, goTo, 6);

		if (transform.position.y < heightToFall)
			cb.Fall();
	}
	void StartMultiplayerRace()
	{
		StartCoroutine (RecalculatePosition ());
	}
	void OnReorderAvatarsByPosition(List<int> players)
	{
		StartCoroutine (RecalculatePosition ());
	}
	IEnumerator RecalculatePosition()
	{
		yield return new WaitForEndOfFrame ();
		print ("__RefreshPosition");
		//this.characterPosition = Game.Instance.level.charactersManager.GetPositionByID(cb.player.id);
		this.characterPosition = cb.player.id;
	}
}
