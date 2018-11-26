using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CharacterBehavior : MonoBehaviour {

	public int team_for_versus;
	public Animation _animation_hero;
	public float speed;
	private bool walking1;
	private bool walking2;

	public Collider[] colliders;
	public CharacterFloorCollitions floorCollitions;

	public states state;

	public enum states
	{
		IDLE,
		RUN,
		JUMP,
		DOUBLEJUMP,
		SUPERJUMP,
		HITTED,
		SHOOT,
		DEAD,
		FALL,
		CRASH
	}
	public CharacterBehavior hasSomeoneOver;
	public CharacterBehavior isOver;
	public CharacterControls controls;
	public CharacterShooter shooter;
	public CharacterMovement characterMovement;

	private int MAX_JETPACK_HEIGHT = 25;

	private float jumpingPressedAmount = 16f;
	float jumpingPressedAmountFactor = 2.5f;
	private float jumpHeight = 900;
	public float superJumpHeight = 1200;
	private Vector3 movement;
	private float hittedTime;
	private float hittedSpeed;
	private Vector3 hittedPosition;

	public Player player;

	public GameObject model;
	public Data data;

	public int jumpsNumber;

	public Transform groundCheck;
	public bool grounded;
	float startJumping;

	//en la carrera muktiplayer:
	//public int position;
	Rigidbody rb;

	// Use this for initialization
	void Awake () {
		rb = GetComponent<Rigidbody> ();
	}

	public void OnAvatarJump()
	{
		startJumping = Time.time;
		grounded = false;
	}

	void Start () {
		
		characterMovement = GetComponent<CharacterMovement> ();
		data = Data.Instance;  
		player = GetComponent<Player>();

		if (!player.isPlaying) {
			GetComponent<Collider> ().enabled = false;
			rb.useGravity = false;
			rb.isKinematic = true;
			return;
		} else {
			rb.useGravity = true;
			rb.mass = 100;
			rb.isKinematic = false;
		}

		data.events.OnVersusTeamWon += OnVersusTeamWon;
		data.events.OnAvatarProgressBarEmpty += OnAvatarProgressBarEmpty;
		data.events.OncharacterCheer += OncharacterCheer;

		data.events.StartMultiplayerRace += StartMultiplayerRace;


		state = states.RUN;
		Run ();

		if (Data.Instance.playMode == Data.PlayModes.VERSUS) {
			controls.EnabledMovements (false);
			_animation_hero.gameObject.transform.localEulerAngles = Vector3.zero;
		} 



//DEBUG: para que el player ultimo nunca muera
		#if UNITY_EDITOR
		if(player.id == 3)
		{
		rb.isKinematic = true;
		rb.useGravity = false;
		GetComponent<Collider>().enabled = false;
			player.fxState = Player.fxStates.SUPER;
			Vector3 pos = transform.localPosition;
			pos.x = 0;
			pos.y = 1;
			transform.localPosition = pos;
		}
#endif


	}
	void OnDestroy ()
	{
		data.events.OnVersusTeamWon -= OnVersusTeamWon;
		data.events.OnAvatarProgressBarEmpty -= OnAvatarProgressBarEmpty;
		data.events.OncharacterCheer -= OncharacterCheer;
		data.events.StartMultiplayerRace -= StartMultiplayerRace;
	}
	void OnVersusTeamWon(int _team_id)
	{
		if (_team_id == team_for_versus) {
			rb.isKinematic = true;
			state = states.DEAD;
		}
	}



	/// <summary>
	/// /////////////////////////////over
	/// </summary>
	public void OnAvatarOverOther(CharacterBehavior other)
	{
		isOver = other;
	}
	void RunOverOther()
	{
		_animation_hero.Play("over");
	}
	public void OnGetRidOfOverAvatar()
	{
		if (hasSomeoneOver != null)
			hasSomeoneOver.OnAvatarFree();
		Reset();
	}
	public void OnGetRidOfBelowAvatar()
	{
		if (isOver != null)
			isOver.Reset();
		Reset();
	}
	public void OnAvatarFree()
	{
		Jump();
		Reset();
	}
	public void OnAvatarStartCarringSomeone(CharacterBehavior other)
	{
		hasSomeoneOver = other;
	}
	public void Reset()
	{
		isOver = null;
		hasSomeoneOver = null;
	}

	void StartMultiplayerRace()
	{
		controls.EnabledMovements (true);
		state = states.RUN;
		Run();
		Data.Instance.events.OnMadRollerFX(MadRollersSFX.types.ENGINES, player.id);
	}


	public void OncharacterCheer()
	{
		if (Random.Range(0, 8) < 2)
			Data.Instance.events.OnMadRollerFX(MadRollersSFX.types.CHEER, player.id);
		
	}

	public void UpdateByController(float rotationY)
	{
		if (state == states.DEAD)
			return;

		if (!grounded &&  startJumping + 0.2f < Time.time ) {
			Vector3 pos = transform.position;
			pos.y += 1;
			grounded = Physics.Linecast (pos, groundCheck.position, 1 << LayerMask.NameToLayer ("Floor"));
			if(grounded)
				OnFloor();
		}

		characterMovement.UpdateByController (rotationY);
	}


	public void setRotation(Vector3 rot)
	{
		if (team_for_versus == 2)
			rot.y += 180;
		if (transform.localEulerAngles == rot) return;
		transform.localEulerAngles = rot;
	}
	public void bump(float damage)
	{
		Die();
	}
	public void bumpWithStaticObject()
	{
		bumperCollision(new Vector3(transform.localRotation.x, transform.position.y, transform.position.z + 5), 1, 10, 10);
	}
	public void bumperCollision(Vector3 hittedPosition, float damage, float bumperSpeed, float bumperDelay)
	{

	}
	public void Revive()
	{		
		Reset();

		if(player!=null && !player.IsDebbugerPlayer())
			rb.useGravity = true;
		
		rb.velocity = Vector3.zero;
		transform.localEulerAngles = Vector3.zero;
		rb.freezeRotation = true;
		state = states.RUN;
		Run();
	}
	public void OnFloor()
	{
		if (state == states.DEAD) return;
		if (state == states.IDLE) return;

	//	print ("___________OnFloor");

		jumpsNumber = 0;
		state = states.RUN;

		if(state == states.RUN) return;

		Data.Instance.events.OnMadRollerFX (MadRollersSFX.types.TOUCH_GROUND, player.id);

		_animation_hero.Play("floorHit");
		Invoke ("OnFloorDone", 0.5f);

	}
	void OnFloorDone()
	{
		if (state == states.RUN) {
			_animation_hero.Play ("run");
			Data.Instance.events.OnMadRollerFX (MadRollersSFX.types.ENGINES, player.id);
		}
	}
	public void Run()
	{
		if (state == states.DEAD) return;
		if (state == states.IDLE) return;
		if(state == states.RUN) return;

		jumpsNumber = 0;
		state = states.RUN;

		if(isOver != null)
			RunOverOther();
		else
			_animation_hero.Play("run");
	}
	public void Slide()
	{
		_animation_hero.Play("slide");
	}
	public void JumpPressed()
	{
		return;
		if (player.transport != null)
			Jetpack();
	}
	public void AllButtonsReleased()
	{
		//if (player.transport != null)
		//	JetpackOff();
	}
	void OnAvatarProgressBarEmpty()
	{
		//if(state == states.JETPACK)
		//	JetpackOff();
	}
	public void Jetpack()
	{
//		return;
//		if (state == states.JETPACK) return;
//
//		_animation_hero.transform.localEulerAngles = new Vector3(40, 0, 0);
//		_animation_hero.Play("jetPack");
//
//		floorCollitions.OnAvatarFly();
//		state = states.JETPACK;
//		player.transport.SetOn();
	}
	public void JetpackOff()
	{
//		_animation_hero.transform.localEulerAngles = new Vector3(20, 0, 0);
//		floorCollitions.OnAvatarFalling();
//
//		if (player.transport)
//			player.transport.SetOff();
//
//		jumpsNumber = 0;
//		Run();
	}
	public void ResetJump()
	{
		if (state == states.DEAD) return;
		//print ("ResetJump");
		state = states.RUN;
		jumpsNumber = 0;
	}
	bool jumpingPressed;

	float jumpingPressedAmountReal;
	public void JumpingPressed()
	{
		if (!jumpingPressed) {
			jumpingPressedAmountReal = jumpingPressedAmount;
			int rand = Random.Range (0, 10);
			if (rand < 6)
				_animation_hero.Play ("jump");
			else if (rand < 8)
				_animation_hero.Play ("jump_right");
			else
				_animation_hero.Play ("jump_left");
			jumpingPressed = true;
		}

		jumpingPressedAmountReal -= jumpingPressedAmountFactor;
		if (jumpingPressedAmountReal < 0)
			jumpingPressedAmountReal = 0;

		Vector3 v = rb.velocity;
		v.y = 0;
		rb.velocity = v;
		Vector3 pos = transform.localPosition;
		pos.y += jumpingPressedAmount*Time.deltaTime;
		transform.localPosition = pos;

		print (jumpingPressedAmountReal);	
	}
	public void Jump()
	{
		jumpingPressed = false;
		if (Game.Instance.state == Game.states.INTRO || state == states.DEAD || state == states.SUPERJUMP) 
			return;	

		//print ("startJumping :" + startJumping + "  Time.time: " + Time.time + "   jumpsNumber: " + jumpsNumber+ " state: " + state);
		
		if (hasSomeoneOver != null)
			OnGetRidOfOverAvatar();
		else if (isOver != null)
			OnGetRidOfBelowAvatar();

		if (player.transport != null && player.transport.isOn) return;

		jumpsNumber++;
		if (jumpsNumber > 4) return;

	//	if(!controls.isAutomata)
	//		data.events.OnAvatarJump (player.id);

		if (state == states.JUMP)
		{
			if (startJumping + 0.2f < Time.time) 
				SuperJump (superJumpHeight, true);
			
			return;			
		}
		else if(state != states.RUN && state != states.SHOOT)
		{
			return;
		}
		if(state == states.JUMP) return;

		rb.velocity = Vector3.zero;
		OnAvatarJump();

		Data.Instance.events.OnMadRollerFX(MadRollersSFX.types.JUMP, player.id);

		rb.AddForce(new Vector3(0, jumpHeight, 0), ForceMode.Impulse);


		state = states.JUMP;
		ResetColliders();
	}
	void ResetColliders()
	{
		GetComponent<Collider>().enabled = false;
		Invoke("ResetCollidersBack", 0.25f);
	}
	void ResetCollidersBack()
	{
		GetComponent<Collider>().enabled = true;
	}
	public void SuperJump(float _superJumpHeight, bool isDoubleJump = false)
	{
		float velocityY = Mathf.Abs(rb.velocity.y)/8;

		if (velocityY < 1 || !isDoubleJump)
			velocityY = 1;
		
		rb.velocity = Vector3.zero;

		OnAvatarJump();
		Data.Instance.events.OnMadRollerFX (MadRollersSFX.types.DOUBLE_JUMP, player.id);

		int rand = Random.Range (0, 10);
		if(rand<5)
			_animation_hero.Play("doubleJump");
		else
			_animation_hero.Play("doubleJump2");

		rb.AddForce( new Vector3(0, (_superJumpHeight ) - (jumpHeight / 10), 0)*velocityY, ForceMode.Impulse);
		state = states.DOUBLEJUMP;
	//	print ("SuperJump");
	}

	public void SuperJumpByBumped(int force , float offsetY, bool dir_forward)
	{
		if (!GetComponent<Collider>().enabled)
			return;
		ResetColliders();
		OnAvatarJump();

		Vector3 pos = transform.localPosition;
		pos.y += offsetY;
		transform.localPosition = pos;
		SuperJump(force);
		state = states.SUPERJUMP;
		Data.Instance.events.OnMadRollerFX (MadRollersSFX.types.DOUBLE_JUMP, player.id);

		if (!dir_forward)
		{
			_animation_hero.Play("rebota");
		}
		else
		{
			_animation_hero.Play("superJump");            
		}

	}
	public void Fall()
	{
		if (state == states.DEAD) return;
		if (state == states.FALL) return;
		state = states.FALL;
		Data.Instance.events.OnSoundFX("FX vox caida01", player.id);
		Data.Instance.events.OnAvatarFall(this);

		if(team_for_versus == 0)
			Game.Instance.gameCamera.OnAvatarFall (this);
		// Die();
	}

	public void HitWithObject(Vector3 objPosition)
	{
		Hit();
	}
	public void Hit()
	{
		if (state == states.DEAD) return;
		SaveDistance();

		Data.Instance.events.OnMadRollerFX(MadRollersSFX.types.CRASH, player.id);

		Data.Instance.events.OnAvatarCrash(this);

		state = states.CRASH;
		rb.velocity = Vector3.zero;
		rb.AddForce(new Vector3(Random.Range(-500,500), 1500, Random.Range(0,-200)), ForceMode.Impulse);
		rb.freezeRotation = false;
		//removeColliders();

		_animation_hero.Play("hit");

		if (player.charactersManager.characters.Count >1) return;
		Invoke("CrashReal", 0.3f);

		if(team_for_versus == 0)
			Game.Instance.gameCamera.OnAvatarCrash (this);
	}
	void CrashReal()
	{
		if (player.charactersManager.getTotalCharacters () == 1) return;

		Data.Instance.GetComponent<FramesController> ().ForceFrameRate (0.1f);
		Data.Instance.events.RalentaTo (1, 0.2f);

	}
	void SaveDistance()
	{
		//if(Data.Instance.playMode == Data.PlayModes.COMPETITION)
		//    SocialEvents.OnFinalDistance(distance);
	}
	public void Die()
	{		
		if(state == states.DEAD) return;

		SaveDistance();

		state = states.DEAD;
	}


//	public void burned(float damage)
//	{
//		//player.removeEnergy(damage);
//		SuperJump( jumpHeight );
//	}

	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "Player2")
		{
			if (state == CharacterBehavior.states.JUMP)
			{
				CharacterBehavior cb = other.gameObject.GetComponent<CharacterBehavior>();
				if (cb != null)
				{
					// if (cb.transform.localPosition.y > characterBehavior.transform.localPosition.y) return;
					if (cb.state != CharacterBehavior.states.RUN) return;
					if (cb.isOver != null) return;
					if (isOver != null) return;

					//print("Player " + player.id + " con " + cb.player.id);
					cb.OnAvatarStartCarringSomeone(this);
					OnAvatarOverOther(cb);
				}
			}
		} 
	}
}

