using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class Texts : MonoBehaviour {
	
	[Serializable]
	public class VideogameDataJson
	{
		public List<VideogameData> videogames;
	}

	void Start () {
		LoadGameData ();
	}
	
	private void LoadGameData()
	{
		string filePath = Application.streamingAssetsPath + "/texts/texts.json";

		print (filePath);

		if (File.Exists (filePath)) {
			string dataAsJson = File.ReadAllText (filePath);
			VideogameDataJson videoGamesDataJson = JsonUtility.FromJson<VideogameDataJson> (dataAsJson);
			VideogamesData videoGamesData = GetComponent<VideogamesData> ();
			int a = 0;
			foreach (VideogameData data in videoGamesData.all) {
				data.name = videoGamesDataJson.videogames [a].name;
				data.credits = videoGamesDataJson.videogames [a].credits;
				a++;
			}
		}
	}
}
