using UnityEngine;
using System.Collections;

public class WeakPlatform : SceneObject {

	Collider collider;
	public GameObject to;
	public int videoGame_ID;

	Color floor_top;
	Color floor_border ;

	Rigidbody rb;
	bool falling;

    public override void OnRestart(Vector3 pos)
    {
		falling = false;
		floor_top = Data.Instance.videogamesData.GetActualVideogameData ().floor_top;
		floor_border = Data.Instance.videogamesData.GetActualVideogameData ().floor_border;

        base.OnRestart(pos);
		
		rb = GetComponent<Rigidbody> ();
		if (rb != null) {
			rb.useGravity = false;
			rb.isKinematic = true;
		}
		collider = GetComponent<Collider> ();
		collider.enabled = true;

		Renderer[] renderers = GetComponentsInChildren<Renderer>();
		int newVideoGameID = Data.Instance.videogamesData.actualID;
		if (newVideoGameID != videoGame_ID) {
			videoGame_ID = newVideoGameID;
			foreach(Renderer r in renderers)
				ChangeMaterials(r);
		}
    }
	void ChangeMaterials(Renderer renderer)
	{
		if(renderer.gameObject.name == "top")
			renderer.material.color = floor_top;
		else
			renderer.material.color = floor_border;
	}
   void OnTriggerEnter(Collider other) 
	{
		if (falling)
			return;
        if (!isActive) return;
		switch (other.tag)
		{
		case "explotion":
                breakOut(other.transform.localPosition);
			break;
		case "destroyable":
            breakOut(other.transform.localPosition);
			break;
		}
        
	}
	public void breakOut(Vector3 impactPosition) {
		
		collider.enabled = false;
        if (!to)
        {
            Fall();
            return;
        }

        float MidX = transform.lossyScale.x / 4;
        float MidZ = transform.lossyScale.z / 4;
                
      
        for (int a = 0; a < 4; a++)
        {
            SceneObject newSO = ObjectPool.instance.GetObjectForType(to.name, false);
            if (!newSO)
            {
                return;
            }
            else
            {
                Vector3 pos = transform.position;
                Vector3 newPos = new Vector3(0, 0, 0);
                switch (a)
                {
                    case 0: newPos = pos + transform.forward * MidZ + transform.right * MidX; break;
                    case 1: newPos = pos + transform.forward * MidZ - transform.right * MidX; break;
                    case 2: newPos = pos - transform.forward * MidZ - transform.right * MidX; break;
                    case 3: newPos = pos - transform.forward * MidZ + transform.right * MidX; break;
                }
				Game.Instance.sceneObjectsManager.AddSceneObject(newSO, newPos);
              //  newSO.Restart(newPos);                  
                newSO.transform.rotation = transform.rotation;
            }
        }
        
        Pool();

	}
    private void Fall()
    {
		falling = true;
		if(rb==null)
			rb = gameObject.AddComponent<Rigidbody>();
		rb.isKinematic = false;
		rb.useGravity = true;
		rb.mass = 20;
		rb.velocity = Vector3.zero;
		Vector3 dir = (Vector3.up * Random.Range(160,310));
		dir += new Vector3 (Random.Range (-5, 5), Random.Range (-5, 5), Random.Range (-5, 5));
		rb.AddForce(dir, ForceMode.Impulse);
		//rb.AddTorque (new Vector3 (0, Random.Range(-10,10), 0), ForceMode.Impulse);
       // Pool();
        //StartCoroutine(FallDown());
    }
	public override void OnPool()
	{
		if (rb == null)
			return;
		rb.useGravity = false;
		rb.isKinematic = true;
	}

}
