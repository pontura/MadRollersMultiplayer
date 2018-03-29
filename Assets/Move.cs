using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {

	public float speed = 2;
	public Vector2 move_left_right;
	public bool moveingLeft;
	public float initialValue;
	private float realSpeed;  
	int direction;
	public bool randomInitial;

	public void Start()
	{
		changeDirection ();
		if (randomInitial) {
			Vector3 pos = transform.localPosition;
			pos.x += (float)Random.Range(move_left_right.x*10, move_left_right.y*10)/10;
			transform.localPosition = pos;
		}
	}
	public void changeDirection()
	{		
		if (!moveingLeft) {
			direction = -1;
		} else
			direction = 1;
		moveingLeft = !moveingLeft;
	}
	void OnDisable()
	{
		Destroy (this);
	}
	void Update()
	{
		Vector3 pos = transform.localPosition;
		if (pos.x < move_left_right.x && moveingLeft)
			changeDirection ();
		else if (pos.x > move_left_right.y && !moveingLeft)
			changeDirection ();
		pos.x += speed * Time.deltaTime * direction;
		transform.localPosition = pos;
	}

}