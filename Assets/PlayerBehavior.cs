using UnityEngine;
using System.Collections;

public class PlayerBehavior : MonoBehaviour {
	
	public AnimationClip runAnimation;
    public AnimationClip jumpPoseAnimation;
	public AnimationClip superJumpingAnimation;
	
	private Animation _animation;
	
	private float rotationSpeed = 50f;
	private CollisionFlags collisionFlags;
	private float verticalSpeed = 0.0f;
	private float moveSpeed = 0.0f;
	private float gravity = 30f;
	private int jumpsOnAir = 0;
	
	private float speedRunSlow = 5f;
	private float speedRun = 7f;
	private float speedExtraRun = 11f;
	
	private float speedX = 5f;	
	
	private float speed = 1;
	private float jumpingExtraSpeed;
	
	//running		0
	//jumping 		1
	//hitted 		2
	//jetPackered 	3
	
	private int state = 0;
	private Vector3 movement;
	private float bumperSpeed;
	private float bumperDelay;
	private Vector3 hittedPosition;
	private Player player;
	
	public float distance; 
	public float lastDistance; 
	public CharacterController controller;
	private Gui gui;
	
	// Use this for initialization
	void Start () {
		player = GetComponent<Player>(); 
		gui = GameObject.Find("GUI").GetComponent<Gui>();
		controller = GetComponent<CharacterController>();
		controller.detectCollisions = true;
		_animation = GetComponent<Animation>();
		
	}
	public void applyForce(float forceAmount)
	{
		Debug.Log("forceAmount: " + forceAmount);
		float force = 1000;
		this.GetComponent<Rigidbody>().AddForce(Vector3.forward * force);
		state = 2;
	}
	public void bumperCollision(Vector3 hittedPosition, float damage, float bumperSpeed, float bumperDelay)
	{
		if(state == 2) return;
		gui.SendMessage ("removeEnergy", damage);
		this.hittedPosition = hittedPosition;
		this.bumperSpeed = bumperSpeed;
		this.bumperDelay= bumperDelay;
		state = 2;
	}
	void Jump()
	{
		if (jumpsOnAir==0)
		{
			jumpingExtraSpeed = 1.8f;
			verticalSpeed = CalculateJumpVerticalSpeed ( 0.21f );
			state = 1;				
			_animation.Play(jumpPoseAnimation.name);
		} else if (jumpsOnAir==1)
		{
			jumpingExtraSpeed = 1.8f;
			verticalSpeed = CalculateJumpVerticalSpeed ( 0.21f );
			state = 1;				
			_animation.Play(superJumpingAnimation.name);
		}
		jumpsOnAir++;
	}
	void Update () {
	}
	
	float  CalculateJumpVerticalSpeed ( float targetJumpHeight  ){
	    return Mathf.Sqrt(2 * targetJumpHeight * gravity);	
		
	}  
	
	bool IsGrounded () {
		
		return (collisionFlags & CollisionFlags.CollidedBelow) != 0;
	}
	
	void ApplyGravity ()
	{	
		if (IsGrounded ())
		{
			jumpsOnAir = 0;
			verticalSpeed = 0.0f;
			if(state != 0)
				_animation.Play(runAnimation.name);
			state = 0;
		}
		else
			verticalSpeed -= gravity * Time.deltaTime;
	}
	public void burned(float jumpHeight,float damage)
	{
		gui.SendMessage ("removeEnergy", damage);
		SuperJump( jumpHeight );
	}
	public void SuperJump(float _superJumpHeight)
	{
		movement = new Vector3(transform.position.x, transform.position.y+1, speed);
		movement *= Time.deltaTime;				 
		collisionFlags = controller.Move(movement);
		
		jumpsOnAir++;
		state = 1;
		verticalSpeed = CalculateJumpVerticalSpeed ( _superJumpHeight );		
		_animation.Play(superJumpingAnimation.name);
		//Debug.Log("SuperJump " + verticalSpeed + " " + Time.time + IsGrounded ());
	}
	
}


