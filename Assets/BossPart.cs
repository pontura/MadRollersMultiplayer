using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPart : MonoBehaviour {

	public Boss boss;
	public GameObject asset;

	public void Init(Boss _boss)
	{
		this.boss = _boss;
	}
	public void OnActivate()
	{
		CancelInvoke ();
		boss.OnPartBroken (this);
		asset.SetActive (false);
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
