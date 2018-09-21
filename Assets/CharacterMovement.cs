﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {

	public types type;
	public enum types
	{
		NORMAL,
		DASHING_FORWARD,
		DASHING_BACK
	}
	float offset_Dash_Z = 6;
	private int heightToFall = -5;
	CharacterBehavior cb;
	public int characterPosition;
	public Vector3 offset;

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
	public void DashForward()
	{
		if (type == types.NORMAL) {
			type = types.DASHING_FORWARD;
			cb._animation_hero.Play("dashForward");
		}
	}
	void Update()
	{
		if (type == types.NORMAL)
			return;
		if (type == types.DASHING_FORWARD) {
			if (offset.z > offset_Dash_Z)
				type = types.DASHING_BACK;
			offset.z += Time.deltaTime * 100;
		} else if (type == types.DASHING_BACK)
		offset.z -= Time.deltaTime * 10;
		if (offset.z < 0) {
			offset.z = 0;
			type = types.NORMAL;
		}
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

		goTo += offset;

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
		//this.characterPosition = Game.Instance.level.charactersManager.GetPositionByID(cb.player.id);
		this.characterPosition = cb.player.id;
		yield return null;
	}
}