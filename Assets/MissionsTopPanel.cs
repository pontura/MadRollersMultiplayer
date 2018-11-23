using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MissionsTopPanel : MonoBehaviour
{
    private Animation anim;
	public Text field;

    void Start()
    {
        anim =  GetComponent<Animation>();
        Data.Instance.events.OnMissionComplete += OnMissionComplete;
        Data.Instance.events.OnListenerDispatcher += OnListenerDispatcher;
		Data.Instance.events.OnMissionProgress += OnMissionProgress;

    }
    void OnDisable()
    {
        Data.Instance.events.OnListenerDispatcher -= OnListenerDispatcher;
        Data.Instance.events.OnMissionComplete -= OnMissionComplete;
		Data.Instance.events.OnMissionProgress -= OnMissionProgress;
    }
    private void OnMissionComplete(int levelID)
    {
        anim.Play("MissionTopClose");
    }
    private void OnListenerDispatcher(string message)
    {
		if (message == "ShowMissionName") {
			anim.Play ("MissionTopOpen");
			//field.text = Data.Instance.missions.MissionActive.description;
		}
    }
	void OnMissionProgress()
	{
		print ("OnMissionProgres");
		anim.Play ("MissionActive");
	}
}
