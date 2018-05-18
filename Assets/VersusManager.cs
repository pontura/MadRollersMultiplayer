using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersusManager : MonoBehaviour {

	public Area[] areas;
	public int id = 0;

	public Area GetArea()
	{
		return areas[id];
	}
}
