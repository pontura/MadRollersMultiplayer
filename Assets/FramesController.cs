using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FramesController : MonoBehaviour {

	IEnumerator ralentaCoroutine;
	float speedEveryFrame;
	float frameRate = 1;

	void Start () {
		//Data.Instance.events.RalentaTo += RalentaTo;
		//Data.Instance.events.ForceFrameRate += ForceFrameRate;
	}
	public void ForceFrameRate(float newFrameRate)
	{
		if (ralentaCoroutine != null)
			StopAllCoroutines ();
		
		this.frameRate = newFrameRate;
		Time.timeScale = frameRate;
	}
	void RalentaTo (float newFrameRate, float speedEveryFrame = 0.01f) {
		this.speedEveryFrame = speedEveryFrame;

		if (ralentaCoroutine != null)
			StopAllCoroutines ();
		
		ralentaCoroutine = OnChangingSpeed (newFrameRate);
		StartCoroutine ( ralentaCoroutine );
	}
	IEnumerator OnChangingSpeed(float newFrameRate)
	{
		float Resto = 0;
		if(newFrameRate<frameRate)
			Resto = -speedEveryFrame;
		else if(newFrameRate>frameRate)
			Resto = speedEveryFrame;
		while (Mathf.Abs (frameRate - newFrameRate) > 0.05f) {
			frameRate += Resto;
			SetNewTimeScale (frameRate);
			yield return new WaitForSecondsRealtime(speedEveryFrame);
		}
		SetNewTimeScale (newFrameRate);
		yield return null;
	}
	void SetNewTimeScale(float newFrameRate)
	{
		if (newFrameRate > 1)
			newFrameRate = 1;
		else if (newFrameRate < 0)
			newFrameRate = 0;
		Time.timeScale = newFrameRate;
	}
}
