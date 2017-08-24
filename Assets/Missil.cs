using UnityEngine;
using System.Collections;

public class Missil : Weapon {

    public GameObject simpleShot;
    public GameObject doubleShot;
    public GameObject tripleShot;

    void Start()
    {
        ResetAll();
        simpleShot.SetActive(true);
    }
    void ResetAll()
    {
        simpleShot.SetActive(false);
        doubleShot.SetActive(false);
        tripleShot.SetActive(false);
    }    
    public void OnChangeWeapon(Weapon.types type)
    {
        this.type = type;
        ResetAll();
        switch (type)
        {
            case types.SIMPLE:
                simpleShot.SetActive(true);
                break;
            case types.DOUBLE:
                doubleShot.SetActive(true);
                break;
            case types.TRIPLE:
                tripleShot.SetActive(true);
                break;
        }
    }
}
