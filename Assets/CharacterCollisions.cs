using UnityEngine;
using System;

public class CharacterCollisions : MonoBehaviour {

    private CharacterBehavior characterBehavior;
    private Player player;
    private bool hitted;

	void Start()
	{
        characterBehavior = gameObject.transform.parent.GetComponent<CharacterBehavior>();
        player = gameObject.transform.parent.GetComponent<Player>();
	}
	void OnTriggerEnter(Collider other) {

        if (characterBehavior.state == CharacterBehavior.states.DEAD) return;
        if (characterBehavior.state == CharacterBehavior.states.CRASH) return;
        if (characterBehavior.state == CharacterBehavior.states.FALL) return;

        if (other.tag == "wall") 
		{
            if (characterBehavior.state == CharacterBehavior.states.SHOOT) return;
            if (player.fxState == Player.fxStates.NORMAL)
            {
                characterBehavior.data.events.AddExplotion(transform.position, Color.red);
                characterBehavior.Hit();
            }
            else
                other.GetComponent<WeakPlatform>().breakOut(transform.position);
        }
        if (other.tag == "destroyable") 
		{
            if (characterBehavior.state == CharacterBehavior.states.SHOOT) return;
            if (player.fxState == Player.fxStates.NORMAL)
                if (!other.GetComponent<Breakable>().dontKillPlayers) 
                    characterBehavior.HitWithObject(other.transform.position);

          //  breakBreakable(other.GetComponent<Breakable>(), other.transform.position);
        }
        else if (other.tag == "floor" && !hitted)
        {
           // print("CRASH  ____  CCCCCCCCCCCC________" + transform.position.y + "  other:   " +  other.transform.position.y);
            if (transform.position.y - other.transform.position.y < 0f)
            {
              //  characterBehavior.Hit();
            }
            else
            {
                hitted = true;
                characterBehavior.SuperJumpByBumped(1200, 0.5f, false);
                Invoke("resetHits", 1);
            }
            if (other.GetComponent<WeakPlatform>())
                other.GetComponent<WeakPlatform>().breakOut(characterBehavior.transform.position);           
        }
        else if (
            other.tag == "enemy"
            && characterBehavior.state != CharacterBehavior.states.JUMP
            && characterBehavior.state != CharacterBehavior.states.DOUBLEJUMP
            && characterBehavior.state != CharacterBehavior.states.SHOOT
            && characterBehavior.state != CharacterBehavior.states.FALL
            )
        {
            if (player.fxState == Player.fxStates.NORMAL && characterBehavior.state != CharacterBehavior.states.JETPACK)
                characterBehavior.Hit();
            other.GetComponent<MmoCharacter>().Die();
        }
    }
    void resetHits()
    {
        hitted = false;
    }
    void breakBreakable(Breakable breakable, Vector3 position)
    {
        try {
                breakable.breakOut(position);
            }
            catch (Exception e)  {
                print("error" + e);
            }  
    }
}
