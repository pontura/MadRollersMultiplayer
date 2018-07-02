using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSettings : MonoBehaviour {

	public GameObject assets;
	public float time_to_init_enemies;

	void OnDisable()
	{
		Destroy (this);
	}

}
