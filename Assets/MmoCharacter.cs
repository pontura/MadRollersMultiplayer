using UnityEngine;
using System.Collections;

public class MmoCharacter : SceneObject
{
    public GameObject weaponContainer;

    public enum states
    {
        IDLE,
        DEAD,
        WALK,
        RUN,
        JUMP,
        WAIT_TO_JUMP,
        FLY
    }

    public states state;

    private Animation _animation;
    private ObjectPool ObjectPool;

    void Start()
    {
        ObjectPool = Data.Instance.sceneObjectsPool;
    }
    void OnEnable()
    {
        _animation = GetComponentInChildren<Animation>();
        
    }
    public override void OnRestart(Vector3 pos)
    {
        
        gameObject.GetComponent<Collider>().enabled = true;
        base.OnRestart(pos);
        state = states.IDLE;
        Invoke("ResetMaterials", 0.2f);
        
    }
    public void EmptyWeapons()
    {
        foreach (Transform child in weaponContainer.transform) Destroy(child.gameObject);
    }
    private Color clothColor;
    void ResetMaterials()
    {
        if (!GetComponent<EnemyShooter>() && weaponContainer.GetComponentsInChildren<Transform>().Length>0)
            EmptyWeapons();

        if (GetComponent<Jump>() && clothColor != Color.black)
        {
            ChangeSkinMaterial(Resources.Load("Materials/EnemyHead_Lila", typeof(Material)) as Material);            
            ChangeClothesColor( Color.black);
            ChangeSkinColor(Color.black);
        }
        else if (clothColor != Color.red)
        {
            ChangeSkinMaterial( Resources.Load("Materials/EnemyHead", typeof(Material)) as Material );
            ChangeClothesColor( Color.red);
            ChangeSkinColor(Color.yellow);
        }
    }
	public void Die() {
		if(state== states.DEAD) return;

        Data.Instance.events.OnSoundFX("enemyDead");

        setScore();

		Missions missions = Data.Instance.GetComponent<Missions>();
		missions.SendMessage ("killGuy", 1);

        _animation.Play("enemyDie");
		state = states.DEAD;

		SendMessage("isDead", SendMessageOptions.DontRequireReceiver);
		
		Vector3 pos = transform.position;
		pos.y+= 2.1f;
		SendMessage("breakOut",pos);

        StartCoroutine(reset());
        gameObject.GetComponent<Collider>().enabled = false;
	}
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "destroyable" || other.gameObject.tag == "enemy")
            Die();
    }
	IEnumerator reset() {
        yield return new WaitForSeconds(0.5f);
        Data.Instance.events.OncharacterCheer();
        Pool();
	}
	public void run() {
        _animation.Play("enemyRun");
        state = states.RUN;
	}
    public void fly()
    {
        _animation.Play("enemyFly");
        state = states.FLY;
    }
    public void walk()
    {
        _animation.Play("enemyWalk");
        state = states.WALK;
    }
	public void idle() {
        _animation.Play("enemyIdle");
        state = states.IDLE;
	}
    public void Shoot()
    {
        _animation.Play("enemyShoot");
        state = states.IDLE;
    }
    public void waitToJump()
    {
        _animation.Play("enemyIdle");
        state = states.WAIT_TO_JUMP;
    }
	public void jump() {
        Data.Instance.events.OnSoundFX("enemyShout");
        _animation.Play("enemyJump");
        state = states.JUMP;
	}
    public void reachFloor()
    {
        //run();
        //SendMessage("OnReachFloor", SendMessageOptions.DontRequireReceiver);
    }
	// Update is called once per frame
    public override void OnSceneObjectUpdate()
    {
        base.OnSceneObjectUpdate();
        SendMessage("OnSceneObjectUpdated", SendMessageOptions.DontRequireReceiver);
	}
    public void ChangeSkinMaterial(Material material)
    {
        SkinnedMeshRenderer skinnedMesh = GetComponentInChildren<SkinnedMeshRenderer>();
        if (skinnedMesh)
             skinnedMesh.material = material;
    }
    public void ChangeClothesColor(Color color)
    {
        clothColor = color;
        SkinnedMeshRenderer skinnedMesh = GetComponentInChildren<SkinnedMeshRenderer>();
        if (skinnedMesh)
        {
            skinnedMesh.materials[1].color = color;
            skinnedMesh.materials[4].color = color;
        }
    }
    public void ChangeSkinColor(Color color)
    {
        SkinnedMeshRenderer skinnedMesh = GetComponentInChildren<SkinnedMeshRenderer>();
        if (skinnedMesh)
        {
            skinnedMesh.materials[2].color = color;
        }
    }
}
