using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour 
{
    public states state;
    public  enum states
    {
        START,
        PLAYING,
        END
    }
    private CharactersManager charactersManager;	
    
	public Vector3 cameraOrientationVector = new Vector3 (0, 4.5f, -0.2f);
    public float rotationX = 40;
    public Vector3 newCameraOrientationVector;
    public bool onExplotion;
	float explotionForce = 0.25f;

    void Awake()
    {
        Data.Instance.events.OnAvatarDie += OnAvatarDie;
        Data.Instance.events.OnChangeMood += OnChangeMood;
        if (Data.Instance.mode == Data.modes.ACCELEROMETER)
			GetComponent<Camera>().rect = new Rect (0, 0, 1, 1);
    }
    void OnDestroy()
    {
        Data.Instance.events.OnAvatarDie -= OnAvatarDie;
        Data.Instance.events.OnChangeMood -= OnChangeMood;
    }
    void OnChangeMood(int id)
    {
        Color cameraColor = Game.Instance.moodManager.GetMood(id).cameraColor;
        GetComponent<Camera>().backgroundColor = cameraColor;
    }
    public void Init() 
	{
        try
        {
             iTween.Stop();
        } catch
        {

        }

        rotationX = 40;
        Vector3 pos = transform.position;
        pos.x = 0;
        pos.y = 0;
        transform.position = pos;
        //if (Application.platform == RuntimePlatform.Android)
        //{
        //    GetComponent<Vignetting>().enabled = false;
        //}
        charactersManager = Game.Instance.GetComponent<CharactersManager>();

        state = states.PLAYING;
        
		newCameraOrientationVector = cameraOrientationVector;

        transform.localEulerAngles = new Vector3(rotationX, 0, 0);
       
	}

    // viene del multiplayer. despues programarlo bien...
    public void setCameraRotationX(float _rotationX)
    {
        //rotationX = _rotationX;
        transform.localEulerAngles = new Vector3(rotationX, 0, 0);
    }
	public void explote(float explotionForce)
	{
		this.explotionForce = explotionForce*1.5f;
		StartCoroutine (DoExplote ());
	}
	public IEnumerator DoExplote () {	

		float delay = 0.03f;
        for (int a = 0; a < 6; a++)
        {
            rotateRandom( Random.Range(-explotionForce, explotionForce) );
            yield return new WaitForSeconds(delay);
        }
        rotateRandom(0);
		
	}
	private void rotateRandom(float explotionForce)
	{
        Vector3 v = transform.localEulerAngles;
        v.z = explotionForce;
        transform.localEulerAngles = v;
	}

	void LateUpdate () 
	{
        if (state == states.END || state == states.START)
        {
            return;
        }

        Vector3 newPos = charactersManager.getPosition();

		newPos += cameraOrientationVector;

        transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime*7);

       // if (charactersManager.getTotalCharacters() > 1) setCameraRotationX(45); else setCameraRotationX(40);
	}
    void OnAvatarDie(CharacterBehavior cb)
    {
        if (Game.Instance.GetComponent<CharactersManager>().getTotalCharacters() > 0) return;
        if (cb.state == CharacterBehavior.states.CRASH)
            OnAvatarCrash(cb);
        else
            OnAvatarFall(cb);
    }
    public void OnAvatarCrash(CharacterBehavior player)
    {
        if (Game.Instance.GetComponent<CharactersManager>().getTotalCharacters()>1) return;
        if (state == states.END) return;
        print("OnAvatarCrash");
        state = states.END;
        iTween.MoveTo(gameObject, iTween.Hash(
            "position", new Vector3(player.transform.localPosition.x, transform.localPosition.y - 1.3f, transform.localPosition.z - 4.1f),
            "time", 2f,
            "easetype", iTween.EaseType.easeOutCubic,
            "looktarget", player.transform
           // "axis", "x"
            ));
    }
    public void OnAvatarFall(CharacterBehavior player)
	{
        if (Game.Instance.GetComponent<CharactersManager>().getTotalCharacters() > 1) return;
        if (state == states.END) return;
        print("OnAvatarFall");
        state = states.END;
        iTween.MoveTo(gameObject, iTween.Hash(
            "position", new Vector3(transform.localPosition.x, transform.localPosition.y+3f, transform.localPosition.z-3.5f),
            "time", 2f,
            "easetype", iTween.EaseType.easeOutCubic,
            "looktarget", player.transform,
            "axis", "x"
            ));
	}
    //public void OnAvatarFall(CharacterBehavior player)
    //{
    //    state = states.END;
    //}
	public void setOrientation(Vector3 vector, float rotation)
	{
	}
    public void fallDown(int fallDownHeight)
    {
    }
}