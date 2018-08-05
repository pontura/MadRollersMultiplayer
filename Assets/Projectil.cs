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
		Vector3 pos = transform.localPosition;

		myDist += Time.deltaTime * speed;
        rotation = transform.localEulerAngles;

		float multiplier = 150 * Time.deltaTime;

		//RECTIFICA
		float gotoRot = 0;
		if(team_for_versus == 2)
			gotoRot = 180;
		else if (rotation.y > 180)
			gotoRot = 360;
		
		rotation.y = Mathf.Lerp(rotation.y , gotoRot, Time.deltaTime*7);
		
       // rotation.y = 0;
        if (pos.y < - 0.8) Destroy();
        else
		if(myDist >= myRange)
		{
            rotation.x += 30 * Time.deltaTime;			
				
            transform.localEulerAngles = rotation;
		}
		pos += transform.forward * 50  * Time.deltaTime;		
		transform.localPosition = pos;
	}
	void OnTriggerEnter(Collider other) 
	{
        if (!isActive) return;
		if(exploted) return;

		switch (other.tag)
		{
            case "wall":
                addExplotionWall();
				SetScore( ScoresManager.score_for_destroying_wall, ScoresManager.types.DESTROY_WALL);
                Destroy();
                break;
			case "floor":
				addExplotion(0.2f);
				SetScore( ScoresManager.score_for_destroying_floor, ScoresManager.types.DESTROY_FLOOR);
				Destroy();
				break;
		case "enemy":
				MmoCharacter enemy = other.gameObject.GetComponent<MmoCharacter> ();

			//esto funca para los bosses:-----------------------
				if (enemy) {
					if (enemy.state == MmoCharacter.states.DEAD)
						return;
					SetScore( ScoresManager.score_for_killing, ScoresManager.types.KILL);
					enemy.Die ();
				} else {
					other.gameObject.SendMessage("breakOut",other.gameObject.transform.position, SendMessageOptions.DontRequireReceiver);
				}
			//---------------------------------------------------

				Destroy();
				break;
			case "destroyable":
				SetScore( ScoresManager.score_for_breaking, ScoresManager.types.BREAKING);
				other.gameObject.SendMessage("breakOut",other.gameObject.transform.position, SendMessageOptions.DontRequireReceiver);
                Destroy();
				break;
			case "boss":
				SetScore( ScoresManager.score_for_boss, ScoresManager.types.BOSS);
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

			//chequea si el projectil es del otro team
			if (team_for_versus == cb.team_for_versus)
				return;
			
			Data.Instance.GetComponent<FramesController> ().ForceFrameRate (0.05f);
			Data.Instance.events.RalentaTo (1, 0.05f);
			cb.Hit ();
			Destroy();
			break;
		}
	}
	void SetScore(int score, ScoresManager.types type)
    {
		Data.Instance.events.OnScoreOn(playerID, transform.position, score, type);
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
