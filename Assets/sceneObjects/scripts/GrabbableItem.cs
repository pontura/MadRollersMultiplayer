using UnityEngine;
using System.Collections;

public class GrabbableItem : SceneObject
{

	public int energy = 1;
    //[HideInInspector]
    public bool hitted;
    [HideInInspector]
    public float sec = 0;

     [HideInInspector]
    public Collider TriggerCollider;
     [HideInInspector]
    public Collider FloorCollider;

    [HideInInspector]
    public Player player;
   // public AudioClip heartClip;

    public override void OnRestart(Vector3 pos)
    {
        player = null;

        if (gameObject.GetComponent<TrailRenderer>())
            gameObject.GetComponent<TrailRenderer>().enabled = true;

        TriggerCollider = gameObject.GetComponent<SphereCollider>();
        FloorCollider = gameObject.GetComponent<BoxCollider>();

        TriggerCollider.enabled = true;
        FloorCollider.enabled = true;

        base.OnRestart(pos);
        hitted = false;
        transform.localEulerAngles = new Vector3(0, 0, 0);

        if (GetComponent<Rigidbody>() && !GetComponent<Rigidbody>().isKinematic)
            GetComponent<Rigidbody>().velocity = Vector3.zero;

        sec = 0;
    }
    public override void OnPool()
    {
        player = null;
    }
    public override void OnSceneObjectUpdate()
    {
		if(hitted)
		{
            if (player == null) return;
            sec += Time.deltaTime * 100;
			Vector3 position = transform.position;
            Vector3 characterPosition = player.transform.position;
			characterPosition.y+=1f;
			characterPosition.z+=1.2f;
			transform.position = Vector3.MoveTowards(position, characterPosition, 18 * Time.deltaTime);
			if(sec>20)
			{
                Data.Instance.events.OnScoreOn(player.id, Vector3.zero, 10);
                Data.Instance.events.OnGrabHeart();
                Data.Instance.GetComponent<MusicManager>().addHeartSound();
                player = null;
                Pool();
			}
		}
	}
    
    private void OnTriggerEnter(Collider other)
    {
        if (!isActive) return;
		if(other.gameObject.CompareTag("Player"))
		{

            if (other.transform.GetComponent<Player>())
                player = other.transform.GetComponent<Player>();
            else
                player = other.transform.parent.GetComponent<Player>();

            if (player.GetComponent<CharacterBehavior>().state == CharacterBehavior.states.DEAD) return;

            if (gameObject.GetComponent<TrailRenderer>())
                gameObject.GetComponent<TrailRenderer>().enabled = false;
			hitted = true;
            TriggerCollider.enabled = false;
            FloorCollider.enabled = false;
		}
	}
    
}
