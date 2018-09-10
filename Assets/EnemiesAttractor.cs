using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesAttractor : MonoBehaviour {
	
	public Projectil projectil;

	void OnTriggerEnter(Collider other)
	{
		
		projectil.StartFollowing (other.gameObject);
	}
}
