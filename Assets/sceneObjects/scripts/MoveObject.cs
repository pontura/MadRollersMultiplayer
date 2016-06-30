using UnityEngine;
using System.Collections;

public class MoveObject : MonoBehaviour {

    public bool rotateOnMovement = true;    	
	public float duration = 2;
    public Vector2 coords;
    public int initialCoordId = 0;

    public void Start()
    {
        loop();        
	}
    private void loop()
    {
        float destX = 0;

        if (initialCoordId == 1)
            destX = coords.x;
        else            
            destX = coords.y;

        if (rotateOnMovement)
        {
            if (destX > transform.position.x)
                GetComponent<SceneObject>().setRotation(new Vector3(0, -90, 0));
            else
                GetComponent<SceneObject>().setRotation(new Vector3(0, 90, 0));
        }


        iTween.MoveTo(gameObject, iTween.Hash(
            "position", new Vector3(destX, transform.position.y, transform.position.z),
            "easetype", iTween.EaseType.linear,
            "time", duration,
            "oncomplete", "loop"
            ));
        if (initialCoordId == 0) initialCoordId = 1;
        else initialCoordId = 0;

    }
}
