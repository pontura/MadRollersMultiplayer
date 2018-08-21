using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideogameBossPanel : MonoBehaviour {

	public states state;
	public enum states
	{
		OFF,
		IDLE,
		LAUGHING,
		ATTACK,
		MAD
	}
	public GameObject panel;
	public Animator anim;
	public Animation animation;

	void Start()
	{
		panel.SetActive (false);
		
		Data.Instance.events.OnGameStart += OnGameStart;
		Data.Instance.events.OnBossActive += OnBossActive;
		Data.Instance.events.OnAvatarDie += OnAvatarDie;
	}
	void OnGameStart()
	{
		StartCoroutine (InitCoroutine ());
	}
	IEnumerator InitCoroutine ()
	{
		panel.SetActive (true);
		anim.Play ("mad");
		yield return new WaitForSeconds (2);
		animation.Play ("videoGameBossOut");
		yield return new WaitForSeconds (1);
		panel.SetActive (false);
	}
	void OnDestroy()
	{
		Data.Instance.events.OnGameStart -= OnGameStart;
		Data.Instance.events.OnBossActive -= OnBossActive;
		Data.Instance.events.OnAvatarDie -= OnAvatarDie;
	}
	void OnBossActive(bool isOn)
	{
		if (isOn) {
			state = states.IDLE;
			panel.SetActive (true);
			Laugh(3);
		} else {			
			Mad (3);
		}
	}
	void OnAvatarDie(CharacterBehavior cb)
	{
		Laugh (1.5f);
	}
	void Idle()
	{
		if (state == states.OFF)
			return;
		if (state == states.IDLE)
			return;
		anim.Play ("idle");
		StartCoroutine (IdleCoroutine());
	}
	void Laugh(float timer)
	{
		if (state == states.MAD)
			return;
		state = states.LAUGHING;
		StartCoroutine (LaughCoroutine(timer));
	}
	void Mad(float timer)
	{
		if (state == states.MAD)
			return;
		state = states.MAD;
		StartCoroutine (MadCoroutine(timer));
	}
	void Attack()
	{
		if (state == states.IDLE || state == states.MAD)
			return;
		state = states.ATTACK;
		StartCoroutine (AttackCoroutine());
	}
	IEnumerator AttackCoroutine ()
	{
		int rand = Random.Range (0, 10);
		if (rand < 5)
			anim.Play ("axe");
		else
			anim.Play ("pinskull_attack");
		yield return new WaitForSeconds (2);
		Idle ();
	}
	IEnumerator IdleCoroutine ()
	{
		yield return new WaitForSeconds (Random.Range(2,5));
		Attack();
	}
	IEnumerator LaughCoroutine (float timer)
	{
		anim.Play ("laugh");

		yield return new WaitForSeconds (timer);
		if (state == states.LAUGHING)
			Idle ();
	}
	IEnumerator MadCoroutine (float timer)
	{
		anim.Play ("mad");
		yield return new WaitForSeconds (timer);
		animation.Play ("videoGameBossOut");
		yield return new WaitForSeconds (1);
		panel.SetActive (false);
	}
}
