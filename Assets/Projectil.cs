using UnityEngine;
using System.Collections;

public class Projectil : SceneObject {

    public int playerID = -1;
	int realSpeed;
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

	public GameObject BulletPlayer0;
	public GameObject BulletPlayer1;
	public GameObject BulletPlayer2;
	public GameObject BulletPlayer3;
	
	void Start () {

	}
    public void SetColor(Color color)
    {
		if (lastColor == color)
			return;
		
        this.color = color;
		lastColor = color;
        meshToColorize.material.color = color;
    }
	int lastPlayerID;
    public override void OnRestart(Vector3 pos)
    {			
		realSpeed = speed;
		target = null;
        level = Game.Instance.level;
        base.OnRestart(pos);

        myDist = 0;
        exploted = false;
		pos.z += 1;

		if (lastPlayerID != playerID) {

			MultiplayerData multiplayerData = Data.Instance.multiplayerData;

			Color playerColor;

			lastPlayerID = playerID;

			BulletPlayer0.SetActive (false);
			BulletPlayer1.SetActive (false);
			BulletPlayer2.SetActive (false);
			BulletPlayer3.SetActive (false);
			if (playerID < 4) {
				playerColor = multiplayerData.colors [playerID];
				switch (playerID) {
				case 0:
					BulletPlayer0.SetActive (true);
					break;
				case 1:
					BulletPlayer1.SetActive (true);
					break;
				case 2:
					BulletPlayer2.SetActive (true);
					break;
				case 3:
					BulletPlayer3.SetActive (true);
					break;

				}
			} else
				playerColor = multiplayerData.colors [4];
		
			playerColor.a = 0.5f;

			GetComponent<TrailRenderer> ().startColor = playerColor;
			GetComponent<TrailRenderer> ().endColor = playerColor;
		}


    }
    
    public override void OnSceneObjectUpdate()
    {
		if (target != null) {
			if (target.transform.position.z < transform.position.z) {
				target = null;
			} else {		
				Vector3 lookAtPos = target.transform.position;
				lookAtPos.y += 1.5f;
				transform.LookAt (lookAtPos);
			}
		}
		Vector3 pos = transform.localPosition;

		myDist += Time.deltaTime * realSpeed;
        rotation = transform.localEulerAngles;

		float multiplier = 150 * Time.deltaTime;

		//RECTIFICA
		float gotoRot = 0;
		if(team_for_versus == 2)
			gotoRot = 180;
		else if (rotation.y > 180)
			gotoRot = 360;

		rotation.y = Mathf.Lerp(rotation.y , gotoRot, Time.deltaTime*4);
		
       // rotation.y = 0;
		if (pos.y < - 0.8) Reset();
        else
		if(myDist >= myRange)
		{
            rotation.x += 15 * Time.deltaTime;					
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
                Reset();
                break;
			case "floor":
				addExplotion(0.2f);
				SetScore( ScoresManager.score_for_destroying_floor, ScoresManager.types.DESTROY_FLOOR);
				Reset();
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

				Reset();
				break;
			case "destroyable":
				SetScore( ScoresManager.score_for_breaking, ScoresManager.types.BREAKING);
				other.gameObject.SendMessage("breakOut",other.gameObject.transform.position, SendMessageOptions.DontRequireReceiver);
				Reset();
				break;
			case "boss":
				SetScore( ScoresManager.score_for_boss, ScoresManager.types.BOSS);
				other.gameObject.SendMessage("breakOut",other.gameObject.transform.position, SendMessageOptions.DontRequireReceiver);
				Reset();
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
			Reset();
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
	void Reset()
    {
		target = null;
        Pool();
    }

	GameObject target;
	public void StartFollowing(GameObject target)
	{
		
		if (this.target)
			return;
		
		realSpeed /= 4;
		
		this.target = target;
	}
}
