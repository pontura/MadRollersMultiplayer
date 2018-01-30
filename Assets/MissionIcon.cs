using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionIcon : MonoBehaviour {

	public GameObject panel;
	public GameObject icon_bomb;
	public GameObject icon_heart;
	public GameObject icon_enemy;

	void Start()
	{
		panel.SetActive (false);
	}
	public void SetOn(Mission mission)
	{
		panel.SetActive (true);
		icon_enemy.SetActive (false);
		icon_heart.SetActive (false);
		icon_bomb.SetActive (false);
		if(mission.hearts>0)
			icon_heart.SetActive (true);
		else if(mission.bombs>0)
			icon_bomb.SetActive (true);
		else if(mission.guys>0)
			icon_enemy.SetActive (true);
		
	}
}
