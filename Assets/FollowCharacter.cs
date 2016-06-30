using UnityEngine;
using System.Collections;

public class FollowCharacter : MonoBehaviour {

	public float speed = 2;
	//private Transform characterTransform;

    public void restarted()
    {
		//characterTransform = GameObject.FindGameObjectWithTag("Player").transform;
	}

   //public void OnSceneObjectUpdate()
   // {
   //     Vector3 pos = transform.position;
   //     pos.x = characterTransform.position.x;

   //     transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime*speed	);
   // }
}

