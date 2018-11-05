using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSettings : MonoBehaviour {

	public string bossModule;

	//despues borrrar:
	public GameObject assets;
	public float time_to_init_enemies;
	public string asset;

	void OnDisable()
	{
		Destroy (this);
	}

}
