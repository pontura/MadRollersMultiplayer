using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableMissile : Powerup {

    public GameObject simpleShot;
    public GameObject doubleShot;
    public GameObject tripleShot;
    Weapon.types type;

    public override void OnRestart(Vector3 pos)
    {
        base.OnRestart(pos);
        int rand = Random.Range(0, 100);
        if (rand < 33)
            type = Weapon.types.SIMPLE;
        else if (rand < 66)
            type = Weapon.types.DOUBLE;
        else
            type = Weapon.types.TRIPLE;
        InitWeapon(type);
    }
    public void InitWeapon(Weapon.types type)
    {
        ResetAll();
        switch (type)
        {
            case Weapon.types.SIMPLE:
                simpleShot.SetActive(true);
                break;
            case Weapon.types.DOUBLE:
                doubleShot.SetActive(true);
                break;
            case Weapon.types.TRIPLE:
                tripleShot.SetActive(true);
                break;
        }
    }
    void ResetAll()
    {
        simpleShot.SetActive(false);
        doubleShot.SetActive(false);
        tripleShot.SetActive(false);
    }
    public override void OnSceneObjectUpdate()
    {
        if (hitted)
        {
            if (sec == 0)
            {
                // _collider.isTrigger = true;
            }
            sec++;
            Vector3 position = transform.position;
            Vector3 characterPosition = player.transform.position;
            characterPosition.y += 1.5f;
            characterPosition.z += 1.5f;
            transform.position = Vector3.MoveTowards(position, characterPosition, 15 * Time.deltaTime);
            if (sec > 13)
            {
                if (player != null && player.id != null)
                    Data.Instance.events.OnChangeWeapon(player.id, type);

                Pool();
            }
        }
    }
}
