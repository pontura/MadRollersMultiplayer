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
       // rotationX = -30;
        myDist = 0;
        exploted = false;
        pos.z += 1;
        transform.position = pos;

		MultiplayerData multiplayerData = Data.Instance.multiplayerData;
		Color playerColor = multiplayerData.colors[playerID];
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

        if (!isActive) return;
		if(exploted) return;

		switch (other.tag)
		{
            case "wall":
                addExplotionWall();
                SetScore(120);
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
                SetScore(50);
				other.gameObject.SendMessage("breakOut",other.gameObject.transform.position, SendMessageOptions.DontRequireReceiver);
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
