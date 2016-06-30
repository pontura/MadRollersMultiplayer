using UnityEngine;
using System.Collections;

public class EnemyShooter : MonoBehaviour {

    public GameObject myProjectile;
    public GameObject projectile_on;
    private MmoCharacter mmoCharacter;
    private bool ready;

    void Start()
    {
        mmoCharacter = GetComponent<MmoCharacter>();
        OnReachFloor();
        ready = false;
        GameObject weaponOn = Instantiate(projectile_on) as GameObject;
        weaponOn.transform.SetParent(mmoCharacter.weaponContainer.transform);
        weaponOn.transform.localScale = Vector3.one;
        weaponOn.transform.localPosition = Vector3.zero;
    }
    void OnDisable()
    {
        Destroy(gameObject.GetComponent("EnemyShooter"));
    }
    public void OnReachFloor()
    {
  
    }
    public void OnSceneObjectUpdated()
    {
        if (ready) return;
        if (!mmoCharacter) return;        
        if (mmoCharacter.state == MmoCharacter.states.DEAD) return;
        if (mmoCharacter.distanceFromCharacter < 10) Shoot();
    }
    void Shoot()
    {
        mmoCharacter.Shoot();
        SceneObject projectil = ObjectPool.instance.GetObjectForType(myProjectile.name, true);

        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z + 4f);
        transform.localPosition += new Vector3(0, 0, -2);

        projectil.Restart(pos);
        Vector3 rot = transform.localEulerAngles;
        rot.x -= 7;
        projectil.transform.localEulerAngles = rot;
        
        ready = true;

        mmoCharacter.EmptyWeapons();

        

        OnDisable();

    }

}

