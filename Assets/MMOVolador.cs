using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMOVolador : MmoCharacter {

	public float Distance_to_react;
	public Animation anim;
	bool isOn;
	float initial_speed=2.5f;
	float speed;
	float acceleration = 10f;

	public override void OnRestart(Vector3 pos)
	{
		speed = initial_speed;
		isOn = false;
		base.OnRestart (pos);
		GetComponent<Rigidbody> ().useGravity = true;
	}
	public override void OnSceneObjectUpdate()
	{
		if ( distanceFromCharacter < Distance_to_react) {
			
			if(!isOn)
			{
				GetComponent<Rigidbody> ().useGravity = false;
				isOn = true;
				anim.Play ("gargola_fly");
			}
			speed += Time.deltaTime*acceleration;

			Vector3 pos = transform.localPosition;
			pos += -transform.forward * Time.deltaTime * speed;
			pos.y +=  Time.deltaTime * speed;
			transform.localPosition = pos;
		}
	}
}
