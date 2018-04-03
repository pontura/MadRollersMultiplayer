using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPart : MonoBehaviour {

	public BossThrower boss;
	public GameObject asset;

	public void Init(BossThrower _boss)
	{
		this.boss = _boss;
	}
	public void Die()
	{
		CancelInvoke ();
		boss.OnPartBroken (this);
	}
	public void OnActive()
	{
		asset.SetActive (false);
		Invoke ("Reactive", 4);
	}
	void Reactive()
	{
		asset.SetActive (true);
	}
}
