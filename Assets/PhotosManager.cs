using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class PhotosManager : MonoBehaviour {
	
	private string FOLDER = "competencias";
    private string COMPETENCIA = "eva-2016";
	
	void Start () {

        var info = new DirectoryInfo(FOLDER + "/" + COMPETENCIA);
        var fileInfo = info.GetFiles();
        ArcadeRanking arcadeRanking = Data.Instance.GetComponent<ArcadeRanking>();
        foreach (FileInfo fileData in fileInfo)
        {
            string fileName = fileData.Name;
            print(fileName);
            string[] scoreNum = fileName.Split("."[0]);
            int score = int.Parse(scoreNum[0]);

            string url = GetFullPathByFolder(FOLDER + "/" + COMPETENCIA, fileName);
            Texture2D winners = LoadLocal(url);            
            arcadeRanking.OnAddHiscore(winners, score);
        }
	}
	public void SavePhoto(Texture2D photo, int score)
    {
        byte[] bytes = photo.EncodeToPNG();
        string path = score.ToString();
		string url = GetFullPathByFolder(FOLDER + "/" + COMPETENCIA, path + ".png");
        File.WriteAllBytes(url, bytes);
		print("GRABA: " + url);
        Texture2D winners = photo;
        Data.Instance.events.OnHiscore(winners, score);
    }
	 public string GetFullPathByFolder(string FolderName, string fileName)
    {
        string folder = Path.Combine(Application.dataPath + "/../", FolderName);
		if (!Directory.Exists(folder))
			Directory.CreateDirectory(folder);
         return Path.Combine(folder, fileName);
    }
	public static Texture2D LoadLocal(string path){
		if (System.IO.File.Exists(path)){
			var bytes = System.IO.File.ReadAllBytes(path);
			Texture2D winners = new Texture2D(1, 1);
			winners.LoadImage(bytes);
			return winners;
		}else{
			Debug.Log("FILE NOT FOUND AT: "+path);
			return null; 
		}
	}
}
