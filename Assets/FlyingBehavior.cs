using UnityEngine;
using System.Collections;

public class FlyingBehavior : MonoBehaviour
{

    public float speed = 3;
    public float randomSpeedDiff = 0;

    private float realSpeed;

    private GameObject inventory;

    void Start()
    {
        if (randomSpeedDiff != 0)
            realSpeed = speed + Random.Range(0, randomSpeedDiff);
        else
            realSpeed = speed;
        
        //rigidbody.useGravity = false;
    }
    //public void isDead()
    //{
    //    Destroy(inventory);
    //}
    public void OnSceneObjectUpdated()
    {
        transform.Translate(-Vector3.forward * realSpeed * Time.deltaTime);
        Dropper droppableSceneObject = GetComponent<Dropper>();
        if (droppableSceneObject != null)
            droppableSceneObject.OnSceneObjectUpdated();
    }

}

