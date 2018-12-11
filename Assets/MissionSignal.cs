using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MissionSignal : MonoBehaviour {

	public GameObject panel;
    public Text title;
	public Text subtitle;

	void Start () {
        Data.Instance.events.OnListenerDispatcher += OnListenerDispatcher;
		SetState(false);
	}
    void OnDestroy()
    {
        Data.Instance.events.OnListenerDispatcher -= OnListenerDispatcher;
    }
	void SetState(bool isOff)
    {
		panel.SetActive (isOff);
    }
    private void OnListenerDispatcher(string message)
    {
		if (message == "LevelFinish")
		{
			title.text = "";
			subtitle.text = "";
			SetState(true);
			Data.Instance.handWriting.WriteTo(title, Data.Instance.videogamesData.GetActualVideogameData().name, DoneText1);
		}
    }
	void DoneText1()
	{
		int missionID = (int)(Data.Instance.GetComponent<Missions> ().MissionActiveID+1);
		Data.Instance.handWriting.WriteTo(subtitle, "MISSION " + missionID, DoneText2);
	}
	void DoneText2()
	{
		StartCoroutine( CloseAfter(2.5f) );
	}
	IEnumerator CloseAfter(float delay)
	{
		yield return StartCoroutine(Utils.CoroutineUtil.WaitForRealSeconds (delay));
		SetState(false);
    }
}
