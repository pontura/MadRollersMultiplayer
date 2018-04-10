using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {

	public bool isVertical;
	public float speed = 2;
	public Vector2 move_left_right;
	public bool moveingLeft;
	public float initialValue;
	private float realSpeed;  
	int direction;
	public bool randomInitial;
	public float randomOffset;
	public bool dontDestroyOnDisable;

	public void Start()
	{
		changeDirection ();
		if (randomInitial) {
			Vector3 pos = transform.localPosition;
			if(isVertical)
				pos.y += (float)Random.Range(move_left_right.x*10, move_left_right.y*10)/10;
			else
				pos.x += (float)Random.Range(move_left_right.x*10, move_left_right.y*10)/10;
			transform.localPosition = pos;
		}
		if (randomOffset > 0) {
			float rand = Random.Range (0, randomOffset * 100) / 100;
			move_left_right = new Vector2 (move_left_right.x, rand);
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
		if(!dontDestroyOnDisable)
			Destroy (this);
	}
	void Update()
	{
		Vector3 pos = transform.localPosition;
		if (isVertical) {
			if (pos.y < move_left_right.x && moveingLeft)
				changeDirection ();
			else if (pos.y > move_left_right.y && !moveingLeft)
				changeDirection ();
			pos.y += speed * Time.deltaTime * direction;
		} else {
			if (pos.x < move_left_right.x && moveingLeft)
				changeDirection ();
			else if (pos.x > move_left_right.y && !moveingLeft)
				changeDirection ();
			pos.x += speed * Time.deltaTime * direction;
		}
		transform.localPosition = pos;
	}

}