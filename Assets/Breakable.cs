using UnityEngine;
using System.Collections;

public class Breakable : MonoBehaviour {

    public ExplotionType explotionType;
    public enum ExplotionType
    {
        SIMPLE,
        BOMB,        
        ENEMY
    }
    public bool isOn;
	public float NumOfParticles = 30;
	private Vector3 position;
	public Breakable[] childs;
	public bool dontKillPlayers;
	public bool dontDieOnHit;
    private Vector3 originalPosition;
    public System.Action OnBreak = delegate { };
    public int score;

    public void OnEnable()
    {               
        isOn = true;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "explotion")
        {
            breakOut(transform.position);
        }
    }
	public void breakOut (Vector3 position) {
        if (!isOn) return;

        OnBreak();

        Data.Instance.events.OnAddObjectExplotion(transform.position, (int)explotionType);
        
		foreach (Breakable breakable in childs)
		{
            if (breakable && breakable.isOn) breakable.hasGravity();
		}
		this.position = position;

		breaker();

        if (dontDieOnHit)
            dontKillPlayers = true;
        else
            Destroy(gameObject);

        isOn = false;
		SendMessage("OnActivate", SendMessageOptions.DontRequireReceiver);
        SendMessage("Die", SendMessageOptions.DontRequireReceiver);
        
	}
	public void hasGravity() {
        isOn = false;
		dontKillPlayers = true;
				
		if(!gameObject.GetComponent<Rigidbody>())
			gameObject.AddComponent<Rigidbody>();
		
		gameObject.GetComponent<Rigidbody>().isKinematic = false;
		gameObject.GetComponent<Rigidbody>().useGravity = true;

        Vector3 newPosition = new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(0.5f, 1.1f), Random.Range(0, 0.9f));
        gameObject.GetComponent<Rigidbody>().AddForce((Time.deltaTime * newPosition) * 2000, ForceMode.Impulse);

        Vector3 rot = new Vector3(Random.Range(-20, 20), Random.Range(-20, 20), Random.Range(-20, 20));
        gameObject.transform.localEulerAngles += rot;

		GetComponent<Collider>().isTrigger = true;
		//StartCoroutine(makeItTrigger());

		SendMessage("OnActivate", SendMessageOptions.DontRequireReceiver);

		if(childs.Length>0)
		{
			foreach (Breakable breakable  in childs)
			{
                if (breakable && breakable.isOn)
					breakable.hasGravity();
			}
		}
	}
	IEnumerator makeItTrigger() {
		yield return new WaitForSeconds(0.9f);
		GetComponent<Collider>().isTrigger = true;
	}
	
	private void breaker(){

		Transform container = Data.Instance.sceneObjectsPool.Scene.transform;
//		MeshRenderer[] all = GetComponentsInChildren<MeshRenderer> ();
//		int id = 0;
//		float force = 500;
//
//		foreach (MeshRenderer mr in all) {
//			
//			Rigidbody rb = mr.gameObject.AddComponent< Rigidbody >();
//			if (rb == null) rb = mr.gameObject.GetComponent<Rigidbody> ();
//			BreakedBlock bb = mr.gameObject.AddComponent< BreakedBlock >();
//
//			rb.mass = 100;
//			rb.useGravity = true;
//			rb.isKinematic = false;
//
//			bb.Init ();
//			mr.transform.SetParent (container);
//			mr.sortingLayerName = "Default";
//			mr.gameObject.AddComponent<BoxCollider> ();
//			mr.transform.localEulerAngles = new Vector3(0, id * (360 / all.Length), 0);
//			Vector3 direction = ((mr.transform.forward * force) + (Vector3.up * (force*2)));
//			mr.GetComponent<Rigidbody>().AddForce(direction, ForceMode.Impulse);
//
//			id++;
//		}
//		return;

		MeshRenderer[] all = GetComponentsInChildren<MeshRenderer> ();
		Material[] materials = new Material[all.Length];
		Vector3[] pos = new Vector3[all.Length];
		int id = 0;
		foreach (MeshRenderer mr in all) {
			materials [id] = mr.material;
			pos [id] = mr.transform.position;
			id++;
		}
		Game.Instance.level.OnAddHeartsByBreaking(transform.position, materials, pos);
	}
}
