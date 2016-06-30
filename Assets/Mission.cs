using UnityEngine;
using System.Collections;

public class Mission: MonoBehaviour {

    public bool isCompetition;
    public int maxScore;
    public int Hiscore;
    public string avatarHiscore;

	public int hearts = 0;
	public int guys = 0;
	public int points = 0;
	public int listeners = 0;
	public int planes = 0;
	public int bombs = 0;
    public int distance = 0;

	public string description;

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
