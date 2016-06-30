using UnityEngine;
using System.Collections;

public class RandomPosition : MonoBehaviour {

	public float randomX;
	
	public Vector3 getPosition (Vector3 pos) {
        if (randomX == 0) return pos;

		float newX = Random.Range(-randomX,randomX);
        pos.x += newX;

        return pos;
	}
}
