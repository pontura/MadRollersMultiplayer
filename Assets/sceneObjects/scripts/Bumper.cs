using UnityEngine;
using System.Collections;

public class Bumper : MonoBehaviour {

	//private float _y;
    public int force = 16;
    public AnimationClip animationClip;
    public bool backwardJump;

	void OnTriggerEnter(Collider other) 
	{
		switch (other.tag)
		{
		case "Player":
			CharacterBehavior ch = other.transform.parent.GetComponent<CharacterBehavior> ();
			ch.SuperJumpByBumped (force * 100, 0.5f, backwardJump);

			Invoke ("Reset", 0.5f);
			break;
		}
	}
}
