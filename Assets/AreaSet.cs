using UnityEngine;
using System.Collections;

public class AreaSet : MonoBehaviour {

    public int competitionsPriority;
    public bool randomize = true;
	public Area[] areas;
	public int totalAreasInSet;
	public Vector3 cameraOrientation;

    [HideInInspector]
    public int id = 0;

	public Vector3 getCameraOrientation ()  {
		if(cameraOrientation.x != 0 || cameraOrientation.y != 0 || cameraOrientation.z != 0)
			return cameraOrientation;
		else
			return new Vector3(0,0,0);
	}

	public Area getArea () {
        Area area;

        Random.seed = (int)System.DateTime.Now.Ticks;

        if (randomize)
            area = areas[Random.Range(0, areas.Length)];
        else
            area = areas[id];

        //Debug.Log(randomize + " area name: " + area.name + " id : " + num + " areas length: " + areas.Length);

        if (id < areas.Length - 1)
            id++;

        print("AREA: " + area.name);

        return area;
	}
}
