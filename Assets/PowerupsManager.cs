﻿using UnityEngine;
using System.Collections;

public class PowerupsManager : MonoBehaviour {

    public SceneObject Invencible;
    public SceneObject Missile;
    private bool powerUpOn;
    private Player player;

	public  void Init()
    {
        Data.Instance.events.OnAddPowerUp += OnAddPowerUp;
        player = Game.Instance.level.charactersManager.character.GetComponent<Player>();
    }    
    public void OnDestroy()
    {
        Data.Instance.events.OnAddPowerUp -= OnAddPowerUp;
    }
    public bool CanBeThrown()
    {
        if (powerUpOn) return false;
        if (player && player.fxState == Player.fxStates.SUPER) return false;

        return true;
    }
    void OnAddPowerUp(Vector3 pos)
    {
        powerUpOn = true;
        SceneObject newSO = null;
        int rand = Random.Range(0, 10); 

        if(rand<50)
            newSO = ObjectPool.instance.GetObjectForType(Missile.name, true);
        else
            newSO = ObjectPool.instance.GetObjectForType(Invencible.name, true);

        if (newSO)
        {
            int force = 600;
            pos.y += 1.2f;
            newSO.Restart(pos);
            newSO.transform.localEulerAngles = Vector3.zero;
            Vector3 direction = ((newSO.transform.forward * force) + (Vector3.up * (force * 1.8f)));
            newSO.GetComponent<Rigidbody>().AddForce(direction, ForceMode.Acceleration);
        }
        Invoke("Reset", 5);
    }
    void Reset()
    {
        powerUpOn = false;
    }
}
