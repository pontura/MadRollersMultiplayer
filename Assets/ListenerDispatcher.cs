using UnityEngine;
using System.Collections;

public class ListenerDispatcher : MonoBehaviour {


    public myEnum message;
    private bool isOn;
    private Data data;

    void Start()
    {
        data = Data.Instance;
    }
    public enum myEnum // your custom enumeration
    {
        ShowMissionId,
        ShowMissionName
    };
	
	void OnTriggerEnter(Collider other) {

		if(other.tag == "Player")
		{
            if (isOn) return;
            isOn = true;

            data.events.ListenerDispatcher(message.ToString());
		}
	}

}

