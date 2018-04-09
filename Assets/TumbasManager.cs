using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TumbasManager : MonoBehaviour {

    private CharactersManager charactersManager;
    private bool isCompetition;
    public float distance;
    public int hiscoreID;
    private List<Hiscores.Hiscore> hiscore;
    private int offset = 80;

    void Start()
    {
//        if (Data.Instance.playMode == Data.PlayModes.COMPETITION)
//        {
//            isCompetition = true;
//            charactersManager = GetComponent<CharactersManager>();
//            if (Data.Instance.isArcade) return;
//            hiscore = Social.Instance.hiscores.levels[0].hiscore;
//            hiscoreID = hiscore.Count - 1;
//        }
    }
    void Update()
    {
//        if (Data.Instance.isArcade) return;
//        if (!isCompetition) return;
//        if (hiscore.Count == 0) return;
//        if (hiscore.Count <= hiscoreID) return;
//        if (hiscore[hiscoreID].score == null) return;
//        try
//        {
//            if (charactersManager.distance + offset > hiscore[hiscoreID].score)
//            {
//                hiscoreID--;
//                if (hiscoreID <= 0)
//                {
//                    hiscoreID = 0;
//                    Debug.Log("GAMASTE");
//                    return;
//                }
//                Data.Instance.events.OnAddTumba(new Vector3(0, 0, charactersManager.distance + offset), hiscore[hiscoreID].username, hiscore[hiscoreID].facebookID);
//            }
//        } catch
//        {
//            Debug.Log("NO hay records");
//        }
    }
}
