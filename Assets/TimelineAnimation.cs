using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class TimelineAnimation : MonoBehaviour {

	public List<TimelineData> timeLineData;
	int id = 0;
	Vector3 initialPosition;

	void Start () {
		initialPosition = transform.localPosition;
		Init ();
	}
	void Init()
	{
		if (timeLineData [id].rotate)
			RotateInTimeLine ();
		else if (timeLineData [id].move)
			MoveInTimeLine ();
	}
	void MoveInTimeLine()
	{
		if (timeLineData [id].duration == 0)
			return;
		iTween.MoveTo(gameObject, iTween.Hash(
			"x", initialPosition.x + timeLineData[id].data.x,
			"y", initialPosition.y + timeLineData[id].data.y,
			"z", initialPosition.z + timeLineData[id].data.z,
			"time", timeLineData[id].duration,
			"easetype", iTween.EaseType.linear,
			//"easetype", timeLineData[id].easetype,
			"oncomplete", "TweenCompleted",
			"onCompleteTarget", this.gameObject
		));
	}
	void RotateInTimeLine()
	{
		if (timeLineData [id].duration == 0)
			return;
		iTween.RotateTo(gameObject, iTween.Hash(
			"rotation", timeLineData[id].data,
			"time", timeLineData[id].duration,
			"easetype", iTween.EaseType.linear,
			//"easetype", timeLineData[id].easetype,
			"oncomplete", "TweenCompleted",
			"onCompleteTarget", this.gameObject
			// "axis", "x"
		));
	}
	void TweenCompleted()
	{
		id++;
		if (id >= timeLineData.Count)
			id = 0;
		Init ();
	}
	void OnDisable()
	{
		iTween.Stop (this.gameObject);
		Destroy(gameObject.GetComponent("TimelineAnimation"));
	}
}