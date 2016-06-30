using UnityEngine;
using System.Collections;

public class InputMobile : InputType {

    private int id;

    public InputMobile(int playerID)
    {
        this.id = playerID;
    }
    public override bool getStart()
    {
        return Input.GetButton("Start" + id);
    }
    public override bool getOpenMenu()
    {
        return Input.GetButton("MainMenu");
    }
    public override float getHorizontal()
    {
        return Input.GetAxisRaw("Horizontal" + id );
    }
    public override bool getFire()
    {
        return Input.GetButton("Fire" + id );
    }
    public override bool getJump()
    {
        return Input.GetButton("Jump" + id );
    }
}
