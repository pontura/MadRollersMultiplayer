using UnityEngine;
using System.Collections;
using AlpacaSound.RetroPixelPro;

public class GameCamera : MonoBehaviour 
{
	public int team_id;
	public RetroPixelPro retroPixelPro;
	public Camera cam;
    public states state;
    public  enum states
    {
		WAITING_TO_TRAVEL,
        START,
        PLAYING,
        END
    }
    private CharactersManager charactersManager;

    public Vector3 startRotation = new Vector3(0, 0,0);
    public Vector3 startPosition = new Vector3(0, 0,0);

	public Vector3 cameraOrientationVector = new Vector3 (0, 4.5f, -0.8f);
	public Vector3 newCameraOrientationVector;

	public Vector3 defaultRotation =  new Vector3 (47,0,0);
	public Vector3 newRotation;
    
    public bool onExplotion;
	float explotionForce = 0.25f;

    public Animation anim;
	public Vector2 defaultResolution = new Vector2(1920,1080);
	public int newH;
	public int newV;
	int pixel_speed_recovery = 14;
	private GameObject flow_target;

	void ChangeResolution()
	{
		retroPixelPro.horizontalResolution =(int) defaultResolution.x;
		retroPixelPro.horizontalResolution =(int) defaultResolution.y;
	}
	void SetPizelPro()
	{
		if (newH == defaultResolution.x && newV == defaultResolution.y)
			return;

		if (newH < defaultResolution.x)
			newH+=pixel_speed_recovery;
		if (newV < defaultResolution.y)
			newV+=pixel_speed_recovery;
		retroPixelPro.horizontalResolution = newH;
		retroPixelPro.verticalResolution = newV;
	}

    void Start()
    {

		newH = retroPixelPro.horizontalResolution;
		newV = retroPixelPro.verticalResolution;

		charactersManager = Game.Instance.GetComponent<CharactersManager>();
       

		cam.transform.localEulerAngles = startRotation;

        Data.Instance.events.StartMultiplayerRace += StartMultiplayerRace;
		Data.Instance.events.OnChangeMood += OnChangeMood;

		if (team_id > 0) {
			state = states.WAITING_TO_TRAVEL;
			Invoke ("Start_Traveling", 2);
			SetOrientation (new Vector4 (0, 0, 0, 0));
			transform.localPosition = new Vector3 (0, 4, Data.Instance.versusManager.area.z_length-3);
			cam.transform.localEulerAngles = new Vector3 (25, 0, 0);
		}
		else {
			state = states.START;
			transform.localPosition = startPosition;
			Vector3 newPos = transform.localPosition;
			newPos.y = 4.5f;
			transform.localPosition = newPos;
			anim.Play ("intro");
		}

    }
	void Start_Traveling()
	{
		state = states.START;
	}
    void OnDestroy()
    {
        Data.Instance.events.StartMultiplayerRace -= StartMultiplayerRace;
        Data.Instance.events.OnChangeMood -= OnChangeMood;
    }
    void StartMultiplayerRace()
    {
        anim.Stop();
        Init();
        state = states.PLAYING;
		cam.transform.localPosition = Vector3.zero;
    }
    void OnChangeMood(int id)
    {
		return;
    }
    public void Init() 
	{
        try
        {
             iTween.Stop();
        } catch
        {

        }

        
		if (flow_target == null) {
			flow_target = new GameObject ();
			flow_target.transform.SetParent (transform.parent);
			flow_target.name = "Camera_TARGET";
		}
		if (team_id > 0) {
			//SetOrientation (new Vector4 (0, 0, 0, 0));
		//	transform.localPosition = new Vector3 (0, 4, Data.Instance.versusManager.area.z_length);
			//cam.transform.localEulerAngles = new Vector3 (25, 0, 0);
		} else {
			transform.localPosition =  new Vector3 (0, 0,transform.localPosition.z);
		}
	}

	public void explote(float explotionForce)
	{
		return;
		this.explotionForce = explotionForce*1.5f;
		StartCoroutine (DoExplote ());
		newH = 1;
		newV = 1;
	}
	bool exploting;
	public IEnumerator DoExplote () {	
		this.exploting = true;
		float delay = 0.03f;
        for (int a = 0; a < 6; a++)
        {
            rotateRandom( Random.Range(-explotionForce, explotionForce) );
            yield return new WaitForSeconds(delay);
        }
        rotateRandom(0);
		this.exploting = false;
	}
	private void rotateRandom(float explotionForce)
	{
		return;
		Vector3 v = cam.transform.localEulerAngles;
        v.z = explotionForce;
		cam.transform.localEulerAngles = v;
	}
	Vector3 newPos;
	int secondsToJump = 5;
	float sec;
	void LookAtFlow()
	{
		Vector3 newPosTarget = flow_target.transform.localPosition;
		newPosTarget.x = Mathf.Lerp(newPosTarget.x, newPos.x, Time.deltaTime*4.5f);
		newPosTarget.z = transform.localPosition.z+6;
		
		newPosTarget.y = 2;
		flow_target.transform.localPosition = newPosTarget;

		Vector3 pos = flow_target.transform.localPosition - transform.localPosition;
		var newRot = Quaternion.LookRotation(pos);

		cam.transform.localRotation = Quaternion.Lerp(cam.transform.localRotation, newRot, Time.deltaTime*10);
	}



	void LateUpdate () 
	{
        if (state == states.START)
        {           
			if(Data.Instance.playMode == Data.PlayModes.VERSUS)
			{
				Vector3 myPos = transform.localPosition;
				Vector3 destPos = new Vector3 (0, 4, -Data.Instance.versusManager.area.z_length-2);
				transform.localPosition = Vector3.Lerp (myPos, destPos, 0.04f);					
			}
			return;
        }
		if (state == states.END || state == states.WAITING_TO_TRAVEL)
        {
            return;
        }

		if (team_id == 0)
			newPos = charactersManager.getPosition ();
		else
			newPos = charactersManager.getPositionByTeam (team_id);

		Vector3 _newPos  = newPos;
		_newPos += newCameraOrientationVector;

		if (_newPos.x < -15) _newPos.x = -15;
		else if (_newPos.x > 15) _newPos.x = 15;

		_newPos.z = Mathf.Lerp (transform.localPosition.z, _newPos.z, Time.deltaTime*20);
		_newPos.x = Mathf.Lerp (transform.localPosition.x, _newPos.x, Time.deltaTime*10);
		_newPos.y = Mathf.Lerp (transform.localPosition.y, _newPos.y, Time.deltaTime*1);

		transform.localPosition = _newPos;
		LookAtFlow ();
	}
    public void OnAvatarCrash(CharacterBehavior player)
    {
		if (Game.Instance.GetComponent<CharactersManager>().getTotalCharacters() > 0) return;
        if (state == states.END) return;
        print("OnAvatarCrash");
        state = states.END;
		iTween.MoveTo(cam.gameObject, iTween.Hash(
            "position", new Vector3(player.transform.localPosition.x, transform.localPosition.y - 3.75f, transform.localPosition.z - 2.1f),
            "time", 3f,
            "easetype", iTween.EaseType.easeOutCubic,
            "looktarget", player.transform
           // "axis", "x"
            ));
    }
    public void OnAvatarFall(CharacterBehavior player)
	{
		
		if (Game.Instance.GetComponent<CharactersManager>().getTotalCharacters() > 0) return;
        if (state == states.END) return;

		print("OnAvatarFall");

        state = states.END;
		iTween.MoveTo(cam.gameObject, iTween.Hash(
            "position", new Vector3(transform.localPosition.x, transform.localPosition.y+3f, transform.localPosition.z-3.5f),
            "time", 2f,
            "easetype", iTween.EaseType.easeOutCubic,
            "looktarget", player.transform,
            "axis", "x"
            ));
	}
	public void SetOrientation(Vector4 orientation)
	{
		print ("CAM SetOrientation_______________________ " + orientation);
		newCameraOrientationVector = cameraOrientationVector + new Vector3 (orientation.x, orientation.y, orientation.z);
		newRotation = defaultRotation + new Vector3 (orientation.w, 0, 0);
	}
    public void fallDown(int fallDownHeight)
    {
    }
}