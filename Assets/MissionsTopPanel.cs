using UnityEngine;
using System.Collections;

public class MissionsTopPanel : MonoBehaviour
{
    private Animation anim;
    void Start()
    {
        anim =  GetComponent<Animation>();
        if (Data.Instance.playMode == Data.PlayModes.STORY)
        {
            Data.Instance.events.OnMissionComplete += OnMissionComplete;
            Data.Instance.events.OnListenerDispatcher += OnListenerDispatcher;
            anim.Play("MissionTopOff");
        } else
            anim.Play("MissionTopOpen");
    }
    void OnDisable()
    {
        Data.Instance.events.OnListenerDispatcher -= OnListenerDispatcher;
        Data.Instance.events.OnMissionComplete -= OnMissionComplete;
    }
    private void OnMissionComplete(int levelID)
    {
        print("_____________OnMissionComplete");
        anim.Play("MissionTopClose");
    }
    private void OnListenerDispatcher(string message)
    {
        print("_______________ShowMissionName");
       if (message == "ShowMissionName" )
           anim.Play("MissionTopOpen");
    }
}
