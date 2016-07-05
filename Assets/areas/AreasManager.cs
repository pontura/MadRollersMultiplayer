using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AreasManager : MonoBehaviour {

	public Area startingArea;
    private int num = 0;

	public List<AreaSet> areaSets;
    public int activeAreaSetID = 1;

	private AreaSet areaSet;

    void Start()
    {
        num = 0;
        activeAreaSetID = 1;
        
    }
    public void RandomizeAreaSetsByPriority()
    {
       // if (Data.Instance.isArcade) return;
        areaSets = Randomize(areaSets);
        areaSets = areaSets.OrderBy(x => x.competitionsPriority).ToList();
        areaSets.Reverse();
    }
    private List<AreaSet> Randomize(List<AreaSet> toRandom)
    {
        SocialEvents.OnCompetitionHiscore(1, 0, false);

        if (!Data.Instance.isArcade && Data.Instance.playMode == Data.PlayModes.COMPETITION)
            Random.seed = Social.Instance.hiscores.GetMyScore();
        for (int i = 0; i < toRandom.Count; i++)
        {
            AreaSet temp = toRandom[i];
            int randomIndex = Random.Range(i, toRandom.Count);
            toRandom[i] = toRandom[randomIndex];
            toRandom[randomIndex] = temp;
        }
        return toRandom;
    }
	public void Init(int _activeAreaSetID)
	{
        activeAreaSetID = 1;

#if UNITY_EDITOR
        if (Data.Instance.DEBUG && Data.Instance.missions.test_mission) return;
        if (Data.Instance.playMode == Data.PlayModes.COMPETITION)
        {
            areaSets.Clear();
            GameObject[] thisAreaSets = Resources.LoadAll<GameObject>("competition_1");
            foreach (GameObject go in thisAreaSets)
            {
                AreaSet thisAreaSet = go.GetComponent<AreaSet>() as AreaSet;
                if (thisAreaSet) areaSets.Add(thisAreaSet);
            }
            RandomizeAreaSetsByPriority();
        }
#endif
        num = 0;
		activeAreaSetID = 0;
		setNewAreaSet();
	}
	
	private int getDifferentLevel()
	{
		return 2;
	}
	private void setNewAreaSet()
	{
        
        //if (areaSet.competitionsPriority == 0)
        //{
        //    activeAreaSetID = Random.Range(2, areaSets.Count - 1);
        //    activeAreaSetID++;
        //}
        if (activeAreaSetID >= areaSets.Count ) activeAreaSetID = areaSets.Count - 1;
        
        areaSet = areaSets[activeAreaSetID];
      //  Debug.Log("NEW AREA: " + areaSet.name + " activeAreaSetID: " + activeAreaSetID);
		//changeCameraOrientation();
        areaSet.id = 0;

        
       
	}
	private void changeCameraOrientation()
	{
		//GameCamera camera = GameObject.Find("Camera").GetComponent<GameCamera>();
		//camera.setOrientation( areaSet.getCameraOrientation(), 23);
	}
	public Area getRandomArea (bool startingArea) {
        num++;
       
        if (!areaSet)
        {
            areaSet = areaSets[0];
            areaSet.id = 0;
        }
		Area area;

      //  print(areaSet + "areaSets.Length: " + areaSets.Count + "  activeAreaSetID: " + activeAreaSetID + " num: " + num + " areaSet.totalAreasInSet " + areaSet.totalAreasInSet);

        if (startingArea)
		{
			area = getStartingArea();
			num = 0;
		} 
		else
		{
            if (Data.Instance.playMode == Data.PlayModes.STORY)
            {
                if (activeAreaSetID < areaSets.Count && num >= areaSet.totalAreasInSet)
                {
                    if (num >= areaSet.totalAreasInSet)
                    {
                        Debug.Log("__setNewAreaSet STORY__" + activeAreaSetID);
                        setNewAreaSet();
                        activeAreaSetID++;
                        num = 0;
                    }
                }
            } else 
            if (num >= areaSet.totalAreasInSet)
                {
                   // Debug.Log("__setNewAreaSet__" + activeAreaSetID);
                    Data.Instance.events.OnSetNewAreaSet(activeAreaSetID);
                    setNewAreaSet();
                //    if (Random.Range(0, 100) < 50) 
                        activeAreaSetID++;
                   // activeAreaSetID++;
                    num = 0;
            }
           
            area = areaSet.getArea();
		}
		return area;
	}
	public Area getRandomSkyArea () {
		Area area = null;
		//area = skyAreas[Random.Range(0, skyAreas.Length)];
		return area;
	}
	public Area getStartingArea()
	{
		return startingArea;
	}
}
