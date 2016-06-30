using UnityEngine;
using System.Collections;

public class DroppableSceneObject : SceneObject {

	private float rotationX;
	private float rotationY;
	private float rotationZ;

    private bool exploted = false;
    public AudioClip coli;

    private float sec = 0;
    private float delay = 0.5f;

    private Collider collider;
    private bool isOn;

    public override void OnRestart(Vector3 pos)
    {
        isOn = false;
        base.OnRestart(pos);
        exploted = false;
        isActive = true;
        collider = GetComponent<Collider>();

    }
    void OnSceneObjectUpdated()
    {
        if (isOn) return;

        if (sec > delay)
        {
            collider.enabled = true;
            isOn = true;
        }
        sec += Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "wall":
                addExplotionWall();
                Destroy();
                AudioSource.PlayClipAtPoint(coli, other.gameObject.transform.position);
                break;
            case "floor":
                addExplotion(0.2f);
                Destroy();
                AudioSource.PlayClipAtPoint(coli, other.gameObject.transform.position);
                break;
            case "enemy":
                MmoCharacter enemy = other.gameObject.GetComponent<MmoCharacter>();
                if (enemy.state == MmoCharacter.states.DEAD) return;

                enemy.Die();
                Destroy();
                AudioSource.PlayClipAtPoint(coli, other.gameObject.transform.position);
                break;
            case "destroyable":
                other.gameObject.SendMessage("breakOut", other.gameObject.transform.position, SendMessageOptions.DontRequireReceiver);
                Destroy();
                AudioSource.PlayClipAtPoint(coli, other.gameObject.transform.position);
                break;
        }
    }
    void addExplotion(float _y)
    {
        if (!isActive) return;
        exploted = true;
        Data.Instance.events.AddExplotion(transform.position, Color.yellow);
    }
    void addExplotionWall()
    {
        if (!isActive) return;
        exploted = true;
        Data.Instance.events.AddWallExplotion(transform.position, Color.yellow);
    }
    void Destroy()
    {
        isActive = false;
        Pool();
    }
}
