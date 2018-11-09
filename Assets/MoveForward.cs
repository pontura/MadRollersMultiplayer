using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour {

	public float speed = 2;
	public float randomSpeedDiff  = 0;

	//para hacer un pingpong
	public float moveBackIn = 0;

	private float realSpeed;

	void Start()
	{
		if (randomSpeedDiff != 0)
			realSpeed = speed + Random.Range(0, randomSpeedDiff);
		else
			realSpeed = speed;

		if (moveBackIn > 0)
			Invoke("LoopRotates", moveBackIn);
	}
	void LoopRotates()
	{
		Vector3 rot = transform.localEulerAngles;
		rot.y += 180;
		transform.localEulerAngles = rot;
		Invoke("LoopRotates", moveBackIn);
	}
	void OnDisable()
	{
		Destroy(gameObject.GetComponent("MoveForward"));
	}

	public void OnSceneObjectUpdated()
	{
		transform.Translate(-Vector3.forward * Mathf.Abs(realSpeed) * Time.deltaTime);
	}
}
