using UnityEngine;
using System.Collections;

public class Bumper : MonoBehaviour {

	//private float _y;
    public int force = 16;
    private CharacterBehavior lastCharacterJumped;
    public AnimationClip animationClip;
    public bool backwardJump;

	void Start()
	{
        lastCharacterJumped = null;
	}

	void OnTriggerEnter(Collider other) 
	{
		switch (other.tag)
		{
		case "Player":
			CharacterBehavior ch = other.transform.parent.GetComponent<CharacterBehavior> ();

			if (lastCharacterJumped == ch)
				return;
			lastCharacterJumped = ch;

			ch.SuperJumpByBumped (force * 100, 0.5f, backwardJump);

			Invoke ("Reset", 0.5f);
			break;
		}
	}
	void Reset()
	{
		lastCharacterJumped = null;
	}
}
