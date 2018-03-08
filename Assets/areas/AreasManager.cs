using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AreasManager : MonoBehaviour { 

    private int num = 0;

	public List<AreaSet> areaSets;
    public int activeAreaSetID = 1;
	[HideInInspector]
	public bool showRelaxAreaBeforeStarting;
	private AreaSet areaSet;

    void Start()
    {
        num = 0;
        activeAreaSetID = 1;        
    }
	public AreaSet GetActiveAreaSet()
	{
		if(activeAreaSetID>areaSets.Count-1)
			return areaSets [activeAreaSetID-1];
		return areaSets [activeAreaSetID];
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
			temp.Restart ();
        }
        return toRandom;
    }
	public void Init(int _activeAreaSetID)
	{
        activeAreaSetID = 1;

//#if UNITY_EDITOR
		if (Data.Instance.DEBUG && Data.Instance.missions.test_mission) {
			setNewAreaSet ();
			return;
		}
        if (Data.Instance.playMode == Data.PlayModes.COMPETITION)
        {
            areaSets.Clear();
            GameObject[] thisAreaSets = Resources.LoadAll<GameObject>("competition_" + Data.Instance.competitionID);
            foreach (GameObject go in thisAreaSets)
            {
                AreaSet thisAreaSet = go.GetComponent<AreaSet>() as AreaSet;
                if (thisAreaSet.competitionsPriority >=99 || thisAreaSet.competitionsPriority<50 || Random.Range(0,10)<7)
                    areaSets.Add(thisAreaSet);
            }
            RandomizeAreaSetsByPriority();
        }
//#endif
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
        if (activeAreaSetID >= areaSets.Count ) 
			activeAreaSetID = areaSets.Count - 1;
        
        areaSet = areaSets[activeAreaSetID];
		print ("_______" + areaSet.name + " " + areaSet.getCameraOrientation ());
		Game.Instance.gameCamera.SetOrientation (areaSet.getCameraOrientation ());

		areaSet.Restart();

       
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
			areaSet.Restart ();
        }
		Area area;

      //  print(areaSet + "areaSets.Length: " + areaSets.Count + "  activeAreaSetID: " + activeAreaSetID + " num: " + num + " areaSet.totalAreasInSet " + areaSet.totalAreasInSet);
		if (Data.Instance.missions.MissionActiveID == 0)
			showRelaxAreaBeforeStarting = false;
        if (startingArea)
		{
			//area = Game.Instance.level.victoryAreaLastLevel;
			area = getStartingArea();
			num = 0;
		} else 	if (showRelaxAreaBeforeStarting) {			
			showRelaxAreaBeforeStarting = false;
			return Data.Instance.missions.relaxArea[Random.Range(0,Data.Instance.missions.relaxArea.Length)];
		} 
		else
		{
            if (Data.Instance.playMode == Data.PlayModes.STORY)
            {
                if (activeAreaSetID < areaSets.Count && num >= areaSet.totalAreasInSet)
                {
                    if (num >= areaSet.totalAreasInSet)
                    {
                        setNewAreaSet();
                        activeAreaSetID++;
                        num = 0;
                    }
                }
            } else 
            if (num >= areaSet.totalAreasInSet)
                {
                    Data.Instance.events.OnSetNewAreaSet(activeAreaSetID);
                    setNewAreaSet();
                    if (Random.Range(0, 10) < 5) 
                        activeAreaSetID++;
                    activeAreaSetID++;
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
		showRelaxAreaBeforeStarting = true;
		float al = Game.Instance.level.areasLength;

		if (al == 0) {
			if (Data.Instance.missions.MissionActiveID == 0)
				return Data.Instance.missions.startingAreaLevel1;
			else
				return Data.Instance.missions.startingArea;
		}
		else
			return Data.Instance.missions.startingAreaDuringGame;
		
	}
}
