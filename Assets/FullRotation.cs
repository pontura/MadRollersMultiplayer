using UnityEngine;
using System.Collections;

public class FullRotation : MonoBehaviour {

	public bool rotateX;
	public bool rotateY;
	public bool rotateZ;
	public bool inverse;

    public bool randomRotation = true;
    public float startRotation = 0;
	
	public float speed = 50f;
    public bool inverseRotation;
	
	private float rotationX;
	private float rotationY;
	private float rotationZ;

    


	void Start () {

        if (randomRotation)
        {
            if (rotateX)
                rotationX = Random.Range(-180, 180);
            if (rotateY)
                rotationY = Random.Range(-45, 45);
            if (rotateZ)
                rotationZ = Random.Range(-45, 45);
        }
        else
        {
            rotationZ = startRotation;
        }

		if(inverse)
			speed *=-1;
	}
	
	void Update () {

		if(rotateX)
			rotationX+=speed*Time.deltaTime;
        else rotationX = transform.localRotation.x;

		if(rotateY)
			rotationY+=speed*Time.deltaTime;
        else rotationY = transform.localRotation.y;

        if (rotateZ && inverseRotation)
			rotationZ+=speed*Time.deltaTime;
        else if (rotateZ && !inverseRotation)
            rotationZ -= speed * Time.deltaTime;
        else rotationZ = transform.localRotation.z;

			
		transform.localRotation = Quaternion.Euler(rotationX, rotationY, rotationZ);
		
	}
}