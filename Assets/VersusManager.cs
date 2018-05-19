using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersusManager : MonoBehaviour {

	public Area[] areas;
	public int id = 0;

	void Start()
	{
		Data.Instance.events.OnVersusTeamWon += OnVersusTeamWon;
	}
	void OnVersusTeamWon(int teamid)
	{
		id++;
		if (id == areas.Length)
			id = 0;
	}
	public Area GetArea()
	{
		return areas[id];
	}
}
