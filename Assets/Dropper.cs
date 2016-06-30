using UnityEngine;
using System.Collections;

public class Dropper : MonoBehaviour
{
    public float delay = 1;
    public float delayRandom = 0;
    public GameObject[] projectiles;

    private float sec;

    public void OnSceneObjectRestarted()
    {
        sec = 0;
        delayRandom = Random.Range(0, delayRandom);
    }
    public void Die()
    {
        
    }

    void OnSceneObjectUpdated()
    {
        if (sec > delay + delayRandom)
        {
            Vector3 pos = transform.position;
            GameObject _projectil = getRandomObject();
            GameObject projectil = Instantiate(_projectil, pos, Quaternion.identity) as GameObject;
            projectil.GetComponent<SceneObject>().Restart(pos);            
            sec = 0;
        }
        sec += Time.deltaTime;
    }
    private GameObject getRandomObject()
    {
        return projectiles[Random.Range(0, projectiles.Length)];
    }
}
