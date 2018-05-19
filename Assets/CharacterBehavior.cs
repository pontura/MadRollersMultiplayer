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
		HITTED,
		SHOOT,
		DEAD,
		FALL,
		CRASH,
		JETPACK,
		JETPACK_OFF
	}
	public CharacterBehavior hasSomeoneOver;
	public CharacterBehavior isOver;
	public CharacterControls controls;

	private int MAX_JETPACK_HEIGHT = 25;

	private int heightToFall = -5;
	private float jumpHeight = 1300;
	public float superJumpHeight = 1200;
	private Vector3 movement;
	private float hittedTime;
	private float hittedSpeed;
	private Vector3 hittedPosition;

	public GameObject myProjectile;

	public Player player;

	public GameObject model;
	public Data data;

	public int jumpsNumber;

	//en la carrera muktiplayer:
	public int position;

	// Use this for initialization
	void Start () {
		data = Data.Instance;       

		player = GetComponent<Player>();

		data.events.OnVersusTeamWon += OnVersusTeamWon;
		data.events.OnAvatarProgressBarEmpty += OnAvatarProgressBarEmpty;
		data.events.OncharacterCheer += OncharacterCheer;
		data.events.OnReorderAvatarsByPosition += OnReorderAvatarsByPosition;
		data.events.StartMultiplayerRace += StartMultiplayerRace;

		Invoke("RefreshPosition", 0.1f);
		//if(Data.Instance.isArcadeMultiplayer)
		//_animation_hero.Play("saluda");

		state = states.JUMP;
		Run ();

		if (Data.Instance.playMode == Data.PlayModes.VERSUS) {
			controls.EnabledMovements (false);
			_animation_hero.gameObject.transform.localEulerAngles = Vector3.zero;
		}
	}
	void OnDestroy ()
	{
		data.events.OnVersusTeamWon -= OnVersusTeamWon;
		data.events.OnAvatarProgressBarEmpty -= OnAvatarProgressBarEmpty;
		data.events.OncharacterCheer -= OncharacterCheer;
		data.events.OnReorderAvatarsByPosition -= OnReorderAvatarsByPosition;
		data.events.StartMultiplayerRace -= StartMultiplayerRace;
	}
	void OnVersusTeamWon(int _team_id)
	{
		if (_team_id == team_for_versus) {
			GetComponent<Rigidbody> ().isKinematic = true;
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
		state = states.JUMP;
		Run();
	}
	void OnReorderAvatarsByPosition(List<int> players)
	{
		Invoke("RefreshPosition", 0.1f);
	}
	void RefreshPosition()
	{
		this.position = Game.Instance.GetComponent<CharactersManager>().GetPositionByID(player.id);
	}
	private float lastShot;
	public void OncharacterCheer()
	{
		if (Random.Range(0, 8) < 2)
		{
			Data.Instance.events.OnSoundFX("FXCheer", player.id);
		}
	}
	public void CheckFire()
	{

		if (_animation_hero)
			_animation_hero.Play("shoot");

		if (state != states.RUN && state != states.SHOOT && transform.localPosition.y<6)
		{
			GetComponent<Rigidbody>().AddForce(new Vector3(0, jumpHeight/3, 0), ForceMode.Impulse);
		}

		Data.Instance.events.OnSoundFX("fire", player.id);

		state = states.SHOOT;

		player.weapon.Shoot();
		if(!controls.isAutomata)
			data.events.OnAvatarShoot(player.id);

		if(lastShot+0.3f > Time.time) return;
		lastShot = Time.time;

		Vector3 pos = new Vector3(transform.position.x, transform.position.y+1.7f, transform.position.z+0.1f);
		OnShoot(pos);
		Invoke("ResetShoot", 0.3f);
	}
	void OnShoot(Vector3 pos)
	{
		switch (player.weapon.type)
		{
		case Weapon.types.SIMPLE:
			Shoot(pos, 0);
			break;
		case Weapon.types.DOUBLE:
			Shoot(new Vector3(pos.x+1, pos.y, pos.z), 0);
			Shoot(new Vector3(pos.x-1, pos.y, pos.z), 0);
			break;
		case Weapon.types.TRIPLE:
			Shoot(pos, 0);
			Shoot(new Vector3(pos.x + 1, pos.y, pos.z), -10);
			Shoot(new Vector3(pos.x - 1, pos.y, pos.z), 10);
			break;
		}

	}
	void Shoot(Vector3 pos, float RotationY)
	{
		Projectil projectil = ObjectPool.instance.GetObjectForType(myProjectile.name, true) as Projectil;

		if (projectil)
		{
			projectil.playerID = player.id;
			projectil.SetColor(player.color);

			projectil.Restart(pos);
			projectil.team_for_versus = team_for_versus;
			Vector3 rot = transform.localEulerAngles;
			rot.x -= 4;

			if (team_for_versus > 1) {
				print(team_for_versus + "  ___________ rota projl");
				rot.y = 180;
			}
			else
				rot.y = RotationY;
			
			projectil.transform.localEulerAngles = rot;
		}
		else
		{
			print("no hay projectil");
		}
	}
	void ResetShoot()
	{
		if (state == states.DEAD)
			return;
		if (floorCollitions.state == CharacterFloorCollitions.states.ON_FLOOR)
			Run();
		else if(jumpsNumber<2)
			state = states.JUMP;
		else
			state = states.DOUBLEJUMP;
	}
	public void UpdateByController(float rotationY)
	{
		if (state == states.DEAD)
			return;
		if (state == states.JETPACK)
		{
			player.OnAvatarProgressBarUnFill(0.25f * Time.deltaTime);
			GetComponent<Rigidbody>().velocity = Vector3.zero;
			GetComponent<Rigidbody>().useGravity = false;

			Vector3 pos = transform.position;

			if (pos.y < MAX_JETPACK_HEIGHT)
			{
				// pos.y += 6 * Time.deltaTime;
				pos.y += (MAX_JETPACK_HEIGHT-pos.y) * Time.deltaTime;

				transform.position = pos;
			}
		}
		else
		{
			//if (transform.position.y > 20 && Random.Range(0,10)<4)
			//  Data.Instance.voicesManager.VoiceSecondaryFromResources("que_vertigo_no");

			GetComponent<Rigidbody>().mass = 100;
			GetComponent<Rigidbody>().useGravity = true;
		}

		Vector3 goTo = transform.position;

		if (isOver)
		{
			goTo.x = isOver.transform.localPosition.x;
			goTo.y = isOver.transform.localPosition.y + 1;
			goTo.z = isOver.transform.localPosition.z+0.2f;
		}
		else
		{
			
			float _z = player.charactersManager.distance - (position / 1);
			if (team_for_versus == 2) {
				rotationY *= -1;
				_z *= -1;
			}
			goTo.x += (rotationY / 3) * Time.deltaTime;
			goTo.z = _z;
		}

		if(controls.ControlsEnabled)
			transform.position = Vector3.Lerp(transform.position, goTo, 6);

		if (transform.position.y < heightToFall)
		{
			Fall();
		}
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
		GetComponent<Rigidbody>().velocity = Vector3.zero;
		GetComponent<Rigidbody>().freezeRotation = true;
		Run();
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
		print("SLIDE:");
		_animation_hero.Play("slide");
	}
	public void JumpPressed()
	{
		if (player.transport != null)
			Jetpack();
	}
	public void AllButtonsReleased()
	{
		if (player.transport != null)
			JetpackOff();
	}
	void OnAvatarProgressBarEmpty()
	{
		if(state == states.JETPACK)
			JetpackOff();
	}
	public void Jetpack()
	{
		if (state == states.JETPACK) return;

		_animation_hero.transform.localEulerAngles = new Vector3(40, 0, 0);
		_animation_hero.Play("jetPack");

		floorCollitions.OnAvatarFly();
		state = states.JETPACK;
		player.transport.SetOn();
	}
	public void JetpackOff()
	{
		_animation_hero.transform.localEulerAngles = new Vector3(20, 0, 0);
		floorCollitions.OnAvatarFalling();

		if (player.transport)
			player.transport.SetOff();

		jumpsNumber = 0;
		Run();
	}
	public void ResetJump()
	{
		if (state == states.DEAD) return;
		state = states.JUMP;
		jumpsNumber = 0;
	}
	public void Jump()
	{
		if (state == states.DEAD) return;
		if (hasSomeoneOver != null)
			OnGetRidOfOverAvatar();
		else if (isOver != null)
			OnGetRidOfBelowAvatar();

		if (player.transport != null && player.transport.isOn) return;

		jumpsNumber++;
		if (jumpsNumber > 4) return;

		if (state == states.JUMP)
		{
			SuperJump(superJumpHeight);
			return;
		}
		else if(state != states.RUN && state != states.SHOOT)
		{
			return;
		}
		if (!player.canJump) return;

		floorCollitions.OnAvatarJump();
		Data.Instance.events.OnSoundFX("FXJump", player.id);

		if(state == states.JUMP) return;

		if(!controls.isAutomata)
			data.events.AvatarJump();

		GetComponent<Rigidbody>().AddForce(new Vector3(0, jumpHeight, 0), ForceMode.Impulse);

		_animation_hero.Play("jump");
		state = states.JUMP;
		ResetColliders();
		return;
	}
	void ResetColliders()
	{
		GetComponent<Collider>().enabled = false;
		Invoke("ResetCollidersBack", 0.5f);
	}
	void ResetCollidersBack()
	{
		GetComponent<Collider>().enabled = true;
	}
	public void SuperJump(float _superJumpHeight)
	{
		if (!player.canJump) return;

		GetComponent<Rigidbody>().velocity = Vector3.zero;

		floorCollitions.OnAvatarJump();
		Data.Instance.events.OnSoundFX("FX jump03 chicle", player.id);

		data.events.AvatarJump();

		_animation_hero.Play("doubleJump");

		GetComponent<Rigidbody>().AddForce(new Vector3(0, (_superJumpHeight ) - (GetComponent<Rigidbody>().velocity.y * (jumpHeight / 10)), 0), ForceMode.Impulse);
		state = states.DOUBLEJUMP;
		return;
	}

	public void SuperJumpByBumped(int force , float offsetY, bool dir_forward)
	{

		ResetColliders();
		floorCollitions.OnAvatarJump();
		data.events.AvatarJump();
		Vector3 pos = transform.localPosition;
		pos.y += offsetY;
		transform.localPosition = pos;
		SuperJump(force);

		Data.Instance.events.OnSoundFX("FX jump03 chicle", player.id);

		if (!dir_forward)
		{
			_animation_hero.Play("rebota");
		}
		else
		{
			_animation_hero.Play("superJump");            
		}

		//lo hago para resetear el doble salto:
		// state = states.JUMP;

	}
	public void Fall()
	{
		if (state == states.DEAD) return;
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

		Data.Instance.events.OnSoundFX("FXCrash", player.id);

		Data.Instance.events.OnAvatarCrash(this);

		state = states.CRASH;
		GetComponent<Rigidbody>().velocity = Vector3.zero;
		GetComponent<Rigidbody>().AddForce(new Vector3(0, 1500, 0), ForceMode.Impulse);
		GetComponent<Rigidbody>().freezeRotation = false;
		//removeColliders();

		_animation_hero.Play("hit");

		if (Game.Instance.GetComponent<CharactersManager>().characters.Count >1) return;
		Invoke("CrashReal", 0.3f);

		if(team_for_versus == 0)
			Game.Instance.gameCamera.OnAvatarCrash (this);
	}
	void CrashReal()
	{
		if (player.charactersManager.getTotalCharacters() == 1) return;
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


	public void burned(float damage)
	{
		//player.removeEnergy(damage);
		SuperJump( jumpHeight );
	}


}

