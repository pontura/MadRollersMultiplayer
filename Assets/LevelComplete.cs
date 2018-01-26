using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelComplete : MonoBehaviour {

	public Stars stars;
    public Text[] fields;

	void Start()
	{
		gameObject.SetActive(false);
	}
     void OnDestroy()
     {
         stars = null;
		fields = null;
     }
    public void Init(int missionNum)
    {
        int maxScore = Data.Instance.GetComponent<Missions>().MissionActive.maxScore;
        int missionScore = Data.Instance.userData.missionScore;
        int quarter = maxScore / 4;

        int starsQty;
        if (missionScore >= quarter*4) starsQty = 3;
        else if (missionScore >= quarter*2) starsQty = 2;
        else if (missionScore >= quarter) starsQty = 1;
        else  starsQty = 0;

        stars.Init(starsQty);
        gameObject.SetActive(true);

		foreach(Text label in fields)
        	label.text = "SCORE " + missionScore;
      
        Data.Instance.events.OnSetStarsToMission(missionNum, starsQty);
    }
}
