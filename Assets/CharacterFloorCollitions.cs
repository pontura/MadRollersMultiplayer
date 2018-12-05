using UnityEngine;
using System.Collections;

public class CharacterFloorCollitions : MonoBehaviour {

    public CharacterBehavior characterBehavior;

	void OnTriggerEnter(Collider other) {
		if (characterBehavior == null)
			return;
        if (characterBehavior.state == CharacterBehavior.states.DEAD
            || characterBehavior.state == CharacterBehavior.states.CRASH
            || characterBehavior.state == CharacterBehavior.states.FALL)
            return;
        if (other.tag == "destroyable")
        {
            if (other.GetComponent<CharacterAnimationForcer>())
                switch (other.GetComponent<CharacterAnimationForcer>().characterAnimation)
                {
                    case CharacterAnimationForcer.animate.SLIDE: characterBehavior.Slide(); break;
                }
        }
        if (other.tag == "enemy")
        {
            if (characterBehavior.state == CharacterBehavior.states.JUMP ||
                characterBehavior.state == CharacterBehavior.states.DOUBLEJUMP ||
                characterBehavior.state == CharacterBehavior.states.SHOOT)
            {
				MmoCharacter mmoCharacter = other.GetComponent<MmoCharacter> ();
				if(mmoCharacter !=null)
              		other.GetComponent<MmoCharacter>().Die();
				else
					other.gameObject.SendMessage("breakOut",other.gameObject.transform.position, SendMessageOptions.DontRequireReceiver);
				
                characterBehavior.SuperJumpByBumped(1200, 0.5f, false);
            }
        }
	}
}
