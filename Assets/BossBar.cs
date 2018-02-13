using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBar : MonoBehaviour {
	
	public GameObject asset;
	public float total;
	Boss boss;

	public void Init(Boss boss)
	{
		this.boss = boss;
		total = 1;
	}

	public void Resta (float qty) {
		total -= qty;
		asset.transform.localScale = new Vector3 (total, 1, 1);
		if (total <= 0)
			boss.Killed ();
	}
}
