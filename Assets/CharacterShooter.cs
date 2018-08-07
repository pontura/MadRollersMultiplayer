using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterShooter : MonoBehaviour {
	
	public GameObject myProjectile;
	public CharacterBehavior characterBehavior;
	float lastShot = 0;
	float timePressing;
	public Weapon.types weawponType;
	public Missil weapon;
	bool isLoadingGun;

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
	void Update()
	{	
		return;

		if (isLoadingGun) {
			float timePressed = Time.time - timePressing;
			Weapon.types newWeawponType;
			if (timePressed < 0.5f )
				newWeawponType= Weapon.types.SIMPLE;
			else if (timePressed < 1f)
				newWeawponType= Weapon.types.DOUBLE;
			else
				newWeawponType= Weapon.types.TRIPLE;
			if (newWeawponType != weawponType) {
				weawponType = newWeawponType;
				weapon.OnChangeWeapon (newWeawponType);
			}
			//weapon.Turn (characterBehavior.transform.eulerAngles.y);
		}
	}
	void OnChangeWeapon(int playerID, Weapon.types type)
	{       
		this.weawponType = type;
		if (playerID != characterBehavior.player.id) return;    
		weapon.OnChangeWeapon(type);
	}
	public void StartPressingFire(){
		isLoadingGun = true;
		timePressing = Time.time;
		weapon.OnChangeWeapon (Weapon.types.SIMPLE);
	}
	public void CheckFire()
	{
		isLoadingGun = false;

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

		OnShoot (pos, weawponType);

		Invoke("ResetShoot", 0.3f);
	}
	void OnShoot(Vector3 pos, Weapon.types type)
	{
		float offsetY = characterBehavior.transform.localEulerAngles.y;
		switch (type)
		{
		case Weapon.types.SIMPLE:
			Shoot(pos, offsetY);
			break;
		case Weapon.types.DOUBLE:
			Shoot(new Vector3(pos.x+1, pos.y, pos.z),-4 + offsetY);
			Shoot(new Vector3(pos.x-1, pos.y, pos.z), 4 + offsetY);
			break;
		case Weapon.types.TRIPLE:
			Shoot(pos, 0);
			Shoot(new Vector3(pos.x + 1, pos.y, pos.z), -20 + offsetY);
			Shoot(new Vector3(pos.x - 1, pos.y, pos.z), 20 + offsetY);
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

			Game.Instance.sceneObjectsManager.AddSceneObject(projectil, pos);
			projectil.team_for_versus = characterBehavior.team_for_versus;
			Vector3 rot = transform.localEulerAngles;
			rot.x -= 4;

			if (characterBehavior.team_for_versus > 1) {
				rot.y += 180;
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
