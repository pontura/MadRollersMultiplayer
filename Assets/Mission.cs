using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mission: MonoBehaviour {

	public int id;
	public BackgroundSideData[] backgroundSides;

	public List<VoicesManager.VoiceData> voices;

	public int videoGameID;
	public GameObject missionIcon;
    public bool isCompetition;
    public int maxScore;
    public int Hiscore;
    public string avatarHiscore;
	public string description;

//	public void Init()
//	{
//		if(backgroundSides.Length>0)
//			Data.Instance.events.OnChangeBackgroundSide (backgroundSides);
//	}
	public void addPoints (float qty) {
       // setPoints(points + (int)qty);
	}
    public void setPoints(int _points)
    {
       // this.points = _points;
    }
	public void reset()
	{
		//points = 0;
	}
}
