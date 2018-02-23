using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideogamesData : MonoBehaviour {

	public int actualID;
	public VideogameData[] all;

	public void ChangeID(int id)
	{
		actualID = id;
	}
	public VideogameData GetActualVideogameData()
	{
		return all [actualID];
	}

}
