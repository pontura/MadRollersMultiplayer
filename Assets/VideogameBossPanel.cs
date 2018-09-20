﻿using System.Collections;
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
		MAD,
		DROPPING_BOMB
	}
	public GameObject panel;
	public Animator anim;
	public Animation animation;

	void Start()
	{
		panel.SetActive (false);
		
		Data.Instance.events.OnGameStart += OnGameStart;
		Data.Instance.events.OnBossActive += OnBossActive;
		Data.Instance.events.OnBossDropBomb += OnBossDropBomb;
		Data.Instance.events.OnAvatarDie += OnAvatarDie;
	}
	void OnGameStart()
	{
		StartCoroutine (InitCoroutine ());
	}
	IEnumerator InitCoroutine ()
	{
		panel.SetActive (true);
		PlayAnim ("mad");
		yield return new WaitForSeconds (2);
		animation.Play ("videoGameBossOut");
		yield return new WaitForSeconds (1);
		panel.SetActive (false);
	}
	void OnDestroy()
	{
		Data.Instance.events.OnGameStart -= OnGameStart;
		Data.Instance.events.OnBossActive -= OnBossActive;
		Data.Instance.events.OnBossDropBomb -= OnBossDropBomb;
		Data.Instance.events.OnAvatarDie -= OnAvatarDie;
	}
	void OnBossDropBomb()
	{		
		if(state == states.OFF)
			StartCoroutine (AxeCoroutine());
		else
			anim.Play ("axe_throw");
	}
	IEnumerator AxeCoroutine ()
	{
		state = states.DROPPING_BOMB;
		PlayAnim ("axe_idle");
		yield return new WaitForEndOfFrame();
		panel.SetActive (true);

		yield return new WaitForSeconds (1f);
		PlayAnim ("axe_throw");
		yield return new WaitForSeconds (1f);
		if (state == states.DROPPING_BOMB) {
			panel.SetActive (false);
			state = states.OFF;
		}			
	}
	void OnBossActive(bool isOn)
	{
		if (isOn) {
			state = states.IDLE;
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
		PlayAnim ("idle");
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
			PlayAnim ("axe");
		else
			PlayAnim ("pinskull_attack");
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
		PlayAnim ("laugh");
		yield return new WaitForEndOfFrame();
		panel.SetActive (true);

		yield return new WaitForSeconds (timer);
		if (state == states.LAUGHING)
			Idle ();
	}
	IEnumerator MadCoroutine (float timer)
	{
		PlayAnim ("mad");
		yield return new WaitForSeconds (timer);
		animation.Play ("videoGameBossOut");
		yield return new WaitForSeconds (1);
		panel.SetActive (false);
		state = states.OFF;
	}
	void PlayAnim(string animName)
	{
		if(anim != null)
			anim.Play (animName);
	}
}
