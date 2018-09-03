using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPart : MonoBehaviour {

	public Boss boss;
	public GameObject asset;
	bool called;

	public void Init(Boss _boss, GameObject bossAsset = null)
	{
		this.boss = _boss;
		if (bossAsset != null) {
			GameObject newGO = Instantiate (bossAsset);
			newGO.transform.SetParent (transform);
			newGO.transform.localScale = Vector3.one;
			newGO.transform.localEulerAngles = Vector3.zero;
			newGO.transform.localPosition = Vector3.zero;
		}
	}
	public void OnActivate()
	{
		if (called)
			return;
		
		called = true;

		CancelInvoke ();
		boss.OnPartBroken (this);
		asset.SetActive (false);
	}
	public void OnActive()
	{
		SendMessage ("OnBossPartActive", SendMessageOptions.DontRequireReceiver);
		asset.SetActive (false);
		Invoke ("Reactive", 4);
	}
	void Reactive()
	{
		asset.SetActive (true);
	}
}
