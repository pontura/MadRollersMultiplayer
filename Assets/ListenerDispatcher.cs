using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ListenerDispatcher : MonoBehaviour {

    public List<int> playersID;
    public myEnum message;
    private Data data;
    private bool ready;

    void Start()
    {
        data = Data.Instance;
    }
    public enum myEnum // your custom enumeration
    {
        ShowMissionId,
        ShowMissionName,
        LevelFinish,
        LevelFinish_easy,
        LevelFinish_medium,
        LevelFinish_hard,
        LevelTransition,
        Ralenta,
        BonusEntrande
    };
	
	void OnTriggerEnter(Collider other) {

		if(other.tag == "Player")
		{
            Player player = other.GetComponentInParent<Player>();
			if (message == myEnum.ShowMissionName)
			{
				if (!ready)
					data.events.ListenerDispatcher("ShowMissionName");
				ready = true;
			} else
            if (message == myEnum.Ralenta)
            {
                if (!ready)
                    data.events.ListenerDispatcher("Ralenta");
                ready = true;
            }
            else if (message == myEnum.BonusEntrande)
            {
                
                if (player == null) return;

                foreach (int playerID in playersID)
                    if (player.id == playerID)
                        return;

                playersID.Add(player.id);

                if (playersID.Count == Game.Instance.level.charactersManager.getTotalCharacters())
                {
                    data.events.ListenerDispatcher("BonusEntrande");
                    print("BPONUSSSS");
                }

                Invoke("Reset", 1);
            }
            else
            {
                
                if (player == null) return;

                foreach (int playerID in playersID)
                    if (player.id == playerID)
                        return;

                playersID.Add(player.id);

                if (other.transform.position.x > 4)
                    data.events.ListenerDispatcher("LevelFinish_hard");
                else if (other.transform.position.x < -4)
                    data.events.ListenerDispatcher("LevelFinish_easy");
                else
                    data.events.ListenerDispatcher("LevelFinish_medium");

                Invoke("Reset", 1);
            }
		}
        
	}
    void OnDisable()
    {
        Reset();
    }
    void Reset()
    {
        ready = false;
        playersID.Clear();
    }

}

