using UnityEngine;
using System.Collections;

public class WeakPlatform : SceneObject {
	
	public GameObject to;
	int videoGameID;
	public GameObject borders;

    public override void OnRestart(Vector3 pos)
    {
        base.OnRestart(pos);
        GetComponent<Collider>().enabled = true;
		Renderer renderer = GetComponentInChildren<Renderer>();
		int newVideoGameID = Data.Instance.videogamesData.actualID;
		if (newVideoGameID != videoGameID) {
			videoGameID = newVideoGameID;
			ChangeMaterials(renderer);
		}
		if (borders != null) {
			foreach(Renderer r in borders.GetComponentsInChildren<Renderer>())
				ChangeMaterials(r);
		}
    }
	void ChangeMaterials(Renderer renderer)
	{
		Material floor_top = Data.Instance.videogamesData.GetActualVideogameData ().floor_top;
		Material floor_border = Data.Instance.videogamesData.GetActualVideogameData ().floor_border;
		renderer.material = floor_top;
		if(renderer.sharedMaterials.Length>1)
			renderer.sharedMaterials[1] = floor_border;
	}
   void OnTriggerEnter(Collider other) 
	{
        if (!isActive) return;
		switch (other.tag)
		{
		case "explotion":


            //achica la cantidad de pisos que rompe:
            //if (other.transform.localScale.x < 1.5f && (gameObject.name == "mediumBlock1_real(Clone)" || gameObject.name == "mediumBlock1_real"))
            //{
            //    Fall();
            //    return;
            //} else
            //if (other.transform.localScale.x < 2f && (gameObject.name == "smallBlock1_real(Clone)" || gameObject.name == "smallBlock1_real"))
            //{
            //    Fall();
            //    return;
            //} else  {
                breakOut(other.transform.localPosition);
           // }
			break;
		case "destroyable":
            breakOut(other.transform.localPosition);
			break;
		}
        
	}
	public void breakOut(Vector3 impactPosition) {


        GetComponent<Collider>().enabled = false;
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
                    
                newSO.Restart(newPos);                  
                newSO.transform.rotation = transform.rotation;
            }
        }
        
        Pool();
       
	}
    private void Fall()
    {
        Pool();
        //StartCoroutine(FallDown());
    }
    //IEnumerator FallDown()
    //{
    //    hasGravity();
    //    yield return new WaitForSeconds(1);
    //    Pool();
    //}

    //public void hasGravity()
    //{
     
    //    gameObject.GetComponent<Collider>().enabled = false;

    //    if (!gameObject.GetComponent<Rigidbody>())
    //    {
    //        gameObject.AddComponent<Rigidbody>();
    //    }

    //    gameObject.rigidbody.isKinematic = false;
    //    gameObject.rigidbody.useGravity = true;

    //   // Vector3 newPosition = new Vector3(Random.Range(-0.05f, 0.05f), Random.Range(0f, 0.2f), Random.Range(0.05f, 0.05f));
    //    //gameObject.rigidbody.AddForce((Time.deltaTime * newPosition) * 2000, ForceMode.Impulse);
    //}

}
