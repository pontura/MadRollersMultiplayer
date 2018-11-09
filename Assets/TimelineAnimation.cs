using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class TimelineAnimation : MonoBehaviour {

	public List<TimelineData> timeLineData;
	int id = 0;
	Vector3 initialPosition;

	void Start () {
		initialPosition = transform.position;
		Init ();
	}
	void Init()
	{
		if (timeLineData [id].rotate)
			RotateInTimeLine ();
		else if (timeLineData [id].move)
			MoveInTimeLine ();
	}
	iTween.EaseType GetEaseType(TimelineData.easetypes type)
	{
		switch (type) {
		case TimelineData.easetypes.IN_OUT:
			return iTween.EaseType.easeInCubic;
		case TimelineData.easetypes.OUT_IN:
			return iTween.EaseType.easeOutCubic;
		default:
			return iTween.EaseType.linear;
		}
	}
	void MoveInTimeLine()
	{
		if (timeLineData [id].duration == 0)
			return;

		print ("MoveInTimeLine " + gameObject.name + timeLineData[id].data.x);
		iTween.MoveTo(gameObject, iTween.Hash(
			"x", initialPosition.x + timeLineData[id].data.x,
			"y", initialPosition.y + timeLineData[id].data.y,
			"z", initialPosition.z + timeLineData[id].data.z,
			"time", timeLineData[id].duration,
			"easetype", GetEaseType(timeLineData[id].easeType),
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
			"easetype", GetEaseType(timeLineData[id].easeType),
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
		print ("OnDisable");
		iTween.Stop (this.gameObject);
	}
}