using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {

	public float speed = 2;
	public Vector2 move_left_right;
	public bool moveingLeft;
	private float realSpeed;  
	int direction;

	public void Start()
	{
		changeDirection ();
	}
	public void changeDirection()
	{		
		if (!moveingLeft) {
			direction = -1;
		} else
			direction = 1;
		moveingLeft = !moveingLeft;
	}

	public void OnSceneObjectUpdated()
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