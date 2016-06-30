using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelComplete : MonoBehaviour {

    [SerializeField]
    Stars stars;
     [SerializeField]
    Text label;

     void OnDestroy()
     {
         stars = null;
         label = null;
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
        label.text = "SCORE " + missionScore;
        print("____ Mission [" + (missionNum-1) + " " + Data.Instance.GetComponent<Missions>().MissionActive.description + "] hiciste : " + missionScore + " puntos ...... su MaxHiscore: " + maxScore);

        Data.Instance.events.OnSetStarsToMission(missionNum, starsQty);
    }
}
