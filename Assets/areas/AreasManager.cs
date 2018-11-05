using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AreasManager : MonoBehaviour { 

	public List<AreaSet> areaSets;

//	[HideInInspector]
//	public bool showRelaxAreaBeforeStarting;

    public void RandomizeAreaSetsByPriority()
    {
//        areaSets = Randomize(areaSets);
//        areaSets = areaSets.OrderBy(x => x.competitionsPriority).ToList();
//        areaSets.Reverse();
    }
    private List<AreaSet> Randomize(List<AreaSet> toRandom)
    {
        SocialEvents.OnCompetitionHiscore(1, 0, false);

        for (int i = 0; i < toRandom.Count; i++)
        {
            AreaSet temp = toRandom[i];
            int randomIndex = Random.Range(i, toRandom.Count);
            toRandom[i] = toRandom[randomIndex];
            toRandom[randomIndex] = temp;
			temp.Restart ();
        }
        return toRandom;
    }
	private int getDifferentLevel()
	{
		return 2;
	}
}
