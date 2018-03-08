using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour 
{
	public Camera cam;
    public states state;
    public  enum states
    {
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

    void Start()
    {
		//print ("Start");
		//newCameraOrientationVector = cameraOrientationVector;
		//newRotation = rotationX;

		cam.transform.localEulerAngles = newRotation;

        state = states.START;
        transform.position = startPosition;
		cam.transform.localEulerAngles = startRotation;

        Data.Instance.events.StartMultiplayerRace += StartMultiplayerRace;
        Data.Instance.events.OnChangeMood += OnChangeMood;

        if (Data.Instance.mode == Data.modes.ACCELEROMETER)
			GetComponent<Camera>().rect = new Rect (0, 0, 1, 1);

		//if (Data.Instance.isArcadeMultiplayer)
			anim.Play ("intro");
		//else
			//anim.Play ("intro_notMultiplayer");

		Vector3 newPos = transform.localPosition;
		newPos.y = 4.5f;
		transform.localPosition = newPos;
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
		if (Data.Instance.playMode == Data.PlayModes.COMPETITION) {
		//	Color cameraColor = Game.Instance.moodManager.GetMood (id).cameraColor;
		//	GetComponent<Camera> ().backgroundColor = cameraColor;
		}
    }
    public void Init() 
	{
        try
        {
             iTween.Stop();
        } catch
        {

        }

        Vector3 pos = transform.position;
        pos.x = 0;
        pos.y = 0;
        transform.position = pos;

        charactersManager = Game.Instance.GetComponent<CharactersManager>();

	}
	public void explote(float explotionForce)
	{
		this.explotionForce = explotionForce*1.5f;
		StartCoroutine (DoExplote ());
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
		Vector3 v = cam.transform.localEulerAngles;
        v.z = explotionForce;
		cam.transform.localEulerAngles = v;
	}

	void LateUpdate () 
	{
        if (state == states.START)
        {
            return;
        }
        if (state == states.END )
        {
            return;
        }

		Vector3 newPos  = charactersManager.getPosition();
		newPos += newCameraOrientationVector;

		newPos.z = Mathf.Lerp (transform.position.z, newPos.z, 0.5f);
		newPos.x = Mathf.Lerp (transform.position.x, newPos.x, Time.deltaTime*7);
		newPos.y = Mathf.Lerp (transform.position.y, newPos.y, Time.deltaTime*10);

		//newPos = Vector3.Lerp (newPos, newCameraOrientationVector, 0.01f);

		transform.position = newPos;

		if(!exploting)
			cam.transform.localEulerAngles = Vector3.Lerp(cam.transform.localEulerAngles, newRotation, 0.05f);
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
		print ("_______________________ " + orientation);
		newCameraOrientationVector = cameraOrientationVector + new Vector3 (orientation.x, orientation.y, orientation.z);
		newRotation = defaultRotation + new Vector3 (orientation.w, 0, 0);
	}
    public void fallDown(int fallDownHeight)
    {
    }
}