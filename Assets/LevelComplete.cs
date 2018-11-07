using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelComplete : MonoBehaviour {

	public GameObject panel;
	public Stars stars;
    public Text[] fields;

	void Start()
	{
		panel.SetActive(false);
	}
     void OnDestroy()
     {
         stars = null;
		fields = null;
     }
    public void Init(int missionNum)
    {
		Data.Instance.events.RalentaTo (0.6f, 0.05f);
		panel.SetActive (true);
		int maxScore = Data.Instance.GetComponent<Missions>().GetActualMissions().maxScore;
      //  int missionScore = Data.Instance.userData.missionScore;
        int quarter = maxScore / 4;

		string titleText ="";

        int starsQty;
		//if (missionScore >= quarter * 4) {
			titleText = "EXCELLENT";
			starsQty = 3;
//		//} else if (missionScore >= quarter * 2) {
//			titleText = "WELL DONE";
//			starsQty = 2;
//		} else if (missionScore >= quarter) {
//			titleText = "OK...";
//			starsQty = 1;
//		} else {
//			titleText = "POOR...";
//			starsQty = 0;
//		}

        stars.Init(starsQty);

		foreach (Text label in fields)
			label.text = titleText; //"SCORE " + missionScore;
      
        Data.Instance.events.OnSetStarsToMission(missionNum, starsQty);

		CloseAfter (3);
    }
	void CloseAfter(float delay)
	{
		StartCoroutine (Closing(delay));
	}
	IEnumerator Closing(float delay)
	{
		yield return StartCoroutine(Utils.CoroutineUtil.WaitForRealSeconds (delay));

		Close ();
	}
	public void Close()
	{
		Data.Instance.events.RalentaTo (1, 0.05f);
		panel.SetActive (false);
		Game.Instance.level.charactersManager.ResetJumps ();
	}
}
