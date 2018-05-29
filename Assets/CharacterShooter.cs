using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterShooter : MonoBehaviour {
	
	public GameObject myProjectile;
	public CharacterBehavior characterBehavior;
	float lastShot = 0;
	float timePressing;

	public Missil weapon;

	void Start()
	{
		ResetWeapons ();
		Data.Instance.events.OnChangeWeapon += OnChangeWeapon;
	}
	void OnDestroy()
	{
		Data.Instance.events.OnChangeWeapon -= OnChangeWeapon;
	}
	void ResetWeapons()
	{
		weapon.ResetAll ();
	}
	void OnChangeWeapon(int playerID, Weapon.types type)
	{       
		if (playerID != characterBehavior.player.id) return;    

		Missil missil =  weapon.GetComponent<Missil>();

		if (missil)
			missil.OnChangeWeapon(type);
	}
	public void StartPressingFire(){
		timePressing = Time.time;
		weapon.OnChangeWeapon (Weapon.types.SIMPLE);
	}
	public void CheckFire()
	{
		float timePressed = Time.time - timePressing;

		if(lastShot+0.2f > Time.time) return;

		ResetWeapons ();

		if(!characterBehavior.controls.isAutomata)
			Data.Instance.events.OnAvatarShoot(characterBehavior.player.id);

		if (characterBehavior.state != CharacterBehavior.states.RUN && characterBehavior.state != CharacterBehavior.states.SHOOT && transform.localPosition.y<6)
			GetComponent<Rigidbody>().AddForce(new Vector3(0, 400, 0), ForceMode.Impulse);

		characterBehavior.state = CharacterBehavior.states.SHOOT;

		if (characterBehavior._animation_hero)
			characterBehavior._animation_hero.Play("shoot");

		characterBehavior.shooter.weapon.Shoot();
		Data.Instance.events.OnSoundFX("fire", characterBehavior.player.id);
		lastShot = Time.time;

		Vector3 pos = new Vector3(transform.position.x, transform.position.y+1.7f, transform.position.z+0.1f);

		Weapon.types weawponType;
		if (timePressed < 0.5f)
			weawponType= Weapon.types.SIMPLE;
		else if (timePressed < 1f)
			weawponType= Weapon.types.DOUBLE;
		else
			weawponType= Weapon.types.TRIPLE;

		OnShoot (pos, weawponType);

		Invoke("ResetShoot", 0.3f);
	}
	void OnShoot(Vector3 pos, Weapon.types type)
	{
		switch (type)
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
			projectil.playerID = characterBehavior.player.id;
			projectil.SetColor(characterBehavior.player.color);

			projectil.Restart(pos);
			projectil.team_for_versus = characterBehavior.team_for_versus;
			Vector3 rot = transform.localEulerAngles;
			rot.x -= 4;

			if (characterBehavior.team_for_versus > 1) {
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
		if (characterBehavior.state == CharacterBehavior.states.DEAD)
			return;
		if (characterBehavior.floorCollitions.state == CharacterFloorCollitions.states.ON_FLOOR)
			characterBehavior.Run();
		else if(characterBehavior.jumpsNumber<2)
			characterBehavior.state = CharacterBehavior.states.JUMP;
		else
			characterBehavior.state = CharacterBehavior.states.DOUBLEJUMP;
	}
}
