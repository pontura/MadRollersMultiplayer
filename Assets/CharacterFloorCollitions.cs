using UnityEngine;
using System.Collections;

public class CharacterFloorCollitions : MonoBehaviour {

    public  CharacterBehavior characterBehavior;
//    private Vector3 offset = new Vector3(0, 3f, 0);
//    private int skip = ~((1 << 9) | (1 << 10) | (1 << 11) | (1 << 12) | (1 << 13) | (1 << 14) | (1 << 15) | (1 << 16) | (1 << 17) | (1 << 18) | (1 << 19));
//    public states state;
//    private Rigidbody rigidbody;
//    public enum states
//    {
//        ON_FLOOR,
//        ON_START_JUMPING,
//        ON_AIR,
//        ON_FLY
//    }
//
//	void Start () {        
//       // characterBehavior = gameObject.transform.parent.GetComponent<CharacterBehavior>();
//      //  rigidbody = characterBehavior.GetComponent<Rigidbody>();
//	}
//    public void OnAvatarFly()
//    {
//        state = states.ON_FLY;
//        characterBehavior.transform.localEulerAngles = new Vector3(0, 0, 0);
//       // Invoke("WaitToResetCollliders", 0.5f);
//    }
//    public void OnAvatarFalling()
//    {
//        state = states.ON_AIR;
//    }
//    public void OnAvatarJump()
//    {
//        state = states.ON_START_JUMPING;
//        characterBehavior.transform.localEulerAngles = new Vector3(0, 0, 0);
//        Invoke("resetStartJumping", 0.3f);
//    }
//    void resetStartJumping()
//    {
//        state = states.ON_AIR;
//    }
//    void Update ()
//    {
//        if (state == states.ON_START_JUMPING) return;
//
//        if (characterBehavior.state == CharacterBehavior.states.DEAD 
//            || characterBehavior.state == CharacterBehavior.states.CRASH
//            || characterBehavior.state == CharacterBehavior.states.FALL) 
//            return;
//
//        if (state == states.ON_FLY) return;
//
//    }
//    
//
	void OnTriggerEnter(Collider other) {
		if (characterBehavior == null)
			return;
       // if (state == states.ON_START_JUMPING) return;
        if (characterBehavior.state == CharacterBehavior.states.DEAD
            || characterBehavior.state == CharacterBehavior.states.CRASH
            || characterBehavior.state == CharacterBehavior.states.FALL)
            return;

     //   if (state == states.ON_FLY) return;

        if (other.tag == "destroyable")
        {
            if (other.GetComponent<CharacterAnimationForcer>())
                switch (other.GetComponent<CharacterAnimationForcer>().characterAnimation)
                {
                    case CharacterAnimationForcer.animate.SLIDE: characterBehavior.Slide(); break;
                }
        }

//       if (other.tag == "floor" && !other.GetComponent<SliderFloor>())
//        {
//            if (transform.parent.gameObject.GetComponent<SliderEffect>())
//                transform.parent.gameObject.GetComponent<SliderEffect>().speed = 0;
//        }
//        else
//        {
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
      //  }
	}
}
