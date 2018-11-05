using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour {

	public float speed = 2;
	public float randomSpeedDiff  = 0;
	private float realSpeed;

	void Start()
	{
		if (randomSpeedDiff != 0)
			realSpeed = speed + Random.Range(0, randomSpeedDiff);
		else
			realSpeed = speed;
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
