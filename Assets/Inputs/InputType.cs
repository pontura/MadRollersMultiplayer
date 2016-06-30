using UnityEngine;
using System.Collections;

public abstract class InputType {
    public abstract bool getStart();
    public abstract float getHorizontal();
    public abstract bool getFire();
    public abstract bool getJump();
    public abstract bool getOpenMenu();
}
