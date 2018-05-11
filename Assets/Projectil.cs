using UnityEngine;
using System.Collections;

public class Projectil : SceneObject {

    public int playerID;
	public int speed = 7;
	public int myRange = 3;
	public int damage = 10;

	private float myDist;
	private bool exploted;

    private Vector3 rotation;
    private Level level;

    private Color color;
    public MeshRenderer meshToColorize;
	public int team_for_versus;
	
	void Start () {

	}
    public void SetColor(Color color)
    {
        this.color = color;
        meshToColorize.material.color = color;
    }
    public override void OnRestart(Vector3 pos)
    {		
        level = Game.Instance.level;
        base.OnRestart(pos);

        myDist = 0;
        exploted = false;
		pos.z += 1;
		transform.localPosition = pos;

		MultiplayerData multiplayerData = Data.Instance.multiplayerData;

		Color playerColor;

		if(playerID<4)
			playerColor = multiplayerData.colors[playerID];
		else
			playerColor = multiplayerData.colors[4];
		
		playerColor.a = 0.5f;

		GetComponent<TrailRenderer> ().startColor = playerColor;
		GetComponent<TrailRenderer> ().endColor = playerColor;


    }
    
    public override void OnSceneObjectUpdate()
    {
		Vector3 pos = transform.position;
		myDist += Time.deltaTime * speed;
        rotation = transform.localEulerAngles;
       // rotation.y = 0;
        if (pos.y < - 0.8) Destroy();
        else
		if(myDist >= myRange)
		{
            rotation.x += 30 * Time.deltaTime;
            transform.localEulerAngles = rotation;
		}
		pos += transform.forward * 50  * Time.deltaTime;
		
		transform.position = pos;
	}
	void OnTriggerEnter(Collider other) 
	{
		print ("_________________" + other.tag);
        if (!isActive) return;
		if(exploted) return;

		switch (other.tag)
		{
            case "wall":
                addExplotionWall();
                SetScore(180);
                Destroy();
                break;
			case "floor":
				addExplotion(0.2f);
                SetScore(100);
				Destroy();
				break;
			case "enemy":
				MmoCharacter enemy= other.gameObject.GetComponent<MmoCharacter>();
				if(enemy.state ==  MmoCharacter.states.DEAD) return;
                SetScore(enemy.score);
				enemy.Die ();
				Destroy();
				break;
			case "destroyable":
                SetScore(70);
				other.gameObject.SendMessage("breakOut",other.gameObject.transform.position, SendMessageOptions.DontRequireReceiver);
                Destroy();
				break;
			case "boss":
				SetScore(120);
				other.gameObject.SendMessage("breakOut",other.gameObject.transform.position, SendMessageOptions.DontRequireReceiver);
				Destroy();
				break;
		case "firewall":
				//SetScore(70);
			//	other.gameObject.SendMessage("breakOut",other.gameObject.transform.position, SendMessageOptions.DontRequireReceiver);
				Vector3 rot = transform.localEulerAngles;
				rot.y += 180+other.gameObject.GetComponentInParent<SceneObject>().transform.localEulerAngles.y;
				transform.localEulerAngles = rot;
				break;
		case "Player":
			if (Data.Instance.playMode != Data.PlayModes.VERSUS)
				return;
			CharacterBehavior cb = other.gameObject.GetComponentInParent<CharacterBehavior> ();
			if (cb == null 
				|| cb.player.id == playerID 
				|| cb.state == CharacterBehavior.states.CRASH
				|| cb.state == CharacterBehavior.states.FALL
				|| cb.state == CharacterBehavior.states.DEAD)
				return;

			//chequea si el projectil es del otro team ( a mejorar)
			if (playerID <2 && cb.player.id<2 || playerID >1 && cb.player.id>2)
				return;
			cb.Hit ();
			Destroy();
			break;
		}
	}
    void SetScore(int score)
    {
        Data.Instance.events.OnSetFinalScore(playerID, transform.position, score);
    }
	void addExplotion(float _y)
	{
        if (!isActive) return;
		exploted = true;        
        Data.Instance.events.AddExplotion(transform.position, color);
	}
    void addExplotionWall()
    {
        if (!isActive) return;
        exploted = true;
        Data.Instance.events.AddWallExplotion(transform.position, color);
    }
    void Destroy()
    {
        Pool();
    }
}
