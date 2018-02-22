using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class VideogameData {

	public int id;
	public string name;
	public BackgroundOnSides[] backgroundOnSides;

	[Serializable]
	public class BackgroundOnSides
	{
		public string name;
		public BackgroundSideData backgroundSide;
	}
}
