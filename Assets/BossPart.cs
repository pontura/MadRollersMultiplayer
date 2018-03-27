using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPart : MonoBehaviour {

	public BossThrower boss;

	public void Init(BossThrower _boss)
	{
		this.boss = _boss;
	}
	public void Die()
	{
		boss.OnPartBroken (this);
	}
}
