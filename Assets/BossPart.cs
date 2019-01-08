using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPart : MonoBehaviour {

	public Boss boss;
	public GameObject asset;
	bool called;

	public void Init(Boss _boss, string bossAssetPath = null)
	{
		this.boss = _boss;
		Utils.RemoveAllChildsIn (transform);
		if (bossAssetPath != null) {
			GameObject newGO = Instantiate(Resources.Load("bosses/" + bossAssetPath, typeof(GameObject))) as GameObject;
			newGO.transform.SetParent (transform);
			newGO.transform.localScale = Vector3.one;
			newGO.transform.localEulerAngles = Vector3.zero;
			newGO.transform.localPosition = Vector3.zero;
		}
	}
	void Update()
	{
		if (transform.position.y < -15)
			OnActivate ();
	}
	public void OnActivate()
	{
		if (called)
			return;
		
		called = true;
		CancelInvoke ();

		if( boss.HasOnlyOneLifeLeft() )
			Data.Instance.events.OnProjectilStartSnappingTarget (transform.position);		

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
