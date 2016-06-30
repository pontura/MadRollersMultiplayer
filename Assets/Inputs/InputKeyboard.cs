using UnityEngine;
using System.Collections;

public class InputKeyboard : InputType{

    private int id;
    
    public InputKeyboard( int playerID )
    {
        this.id = playerID+1;
    }
    public override bool getOpenMenu()
    {
        return Input.GetButtonDown("MainMenu");
    }
    public override float getHorizontal()
    {
       return Input.GetAxisRaw("Horizontal" + id);
    }
    public override bool getStart()
    {
        return Input.GetButtonDown("Start" + id);
    }
    public override bool getFire()
    {
        return Input.GetButtonDown("Fire" + id);
    }
    public override bool getJump()
    {
       return Input.GetButtonDown("Jump" + id);
    }
}
