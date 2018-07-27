using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMOVolador : MmoCharacter {

	public float Distance_to_react;
	public Animation anim;
	bool isOn;

	public override void OnSceneObjectUpdate()
	{
		if (!isOn && distanceFromCharacter < Distance_to_react) {
			isOn = true;
			anim.Play ("on");
		}
	}
}
