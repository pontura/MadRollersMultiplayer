using UnityEngine;
using System.Collections;

public class Mission: MonoBehaviour {

	public int id;
	public BackgroundSideData[] backgroundSides;

	public int videoGameID;
	public GameObject missionIcon;
    public bool isCompetition;
    public int maxScore;
    public int Hiscore;
    public string avatarHiscore;

	public int boss1 = 0;
	public int hearts = 0;
	public int guys = 0;
	public int ghost = 0;
	public int points = 0;
	public int listeners = 0;
	public int planes = 0;
	public int bombs = 0;
    public int distance = 0;

	public string description;

	public void Init()
	{
		//print ("Init Mission ___________________" + missionIcon.name);
		if(backgroundSides.Length>0)
			Data.Instance.events.OnChangeBackgroundSide (backgroundSides);
	}
	public void addPoints (float qty) {
        setPoints(points + (int)qty);
	}
    public void setPoints(int _points)
    {
        this.points = _points;
    }
	public void reset()
	{
		points = 0;
	}
}
