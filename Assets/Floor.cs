using UnityEngine;
using System.Collections;

public class Floor : MonoBehaviour
{

    [SerializeField]
    GameObject[] areas;
    public int z_length;
    private bool isMoving;
	public float speed = 2;

    private CharactersManager charactersManager;

    public void Init(CharactersManager charactersManager)
    {
        isMoving = true;
        Data.Instance.events.OnGamePaused += OnGamePaused;
        Data.Instance.events.OnAvatarCrash += OnAvatarCrash;
        Data.Instance.events.OnAvatarFall += OnAvatarCrash;
      //  Data.Instance.events.OnChangeMood += OnChangeMood;
        this.charactersManager = charactersManager;
    }
    void OnDestroy()
    {
        Data.Instance.events.OnGamePaused -= OnGamePaused;
        Data.Instance.events.OnAvatarCrash -= OnAvatarCrash;
        Data.Instance.events.OnAvatarFall -= OnAvatarCrash;
      //  Data.Instance.events.OnChangeMood -= OnChangeMood;
    }
    void OnGamePaused(bool paused)
    {
        isMoving = !paused;
    }
    void OnChangeMood(int id)
    {
		return;
      //  string texture = Game.Instance.moodManager.GetMood(id).floorTexture;
        
        foreach (GameObject area in areas)
        {
       //     Material mat = Resources.Load("Materials/Floors/" + texture, typeof(Material)) as Material;
        //    area.GetComponent<MeshRenderer>().material = mat;
        }

    }
    void OnAvatarCrash(CharacterBehavior cb)
    {
        isMoving = false;
    }

	float pos_z;
	float lastCharactersDistance;
    void Update()
    {
        if (!isMoving) return;
        if (!charactersManager) return;
		float charactersDistance = charactersManager.getDistance ();
		if (charactersDistance == lastCharactersDistance)
			return;
		
		lastCharactersDistance = charactersDistance;

		pos_z += Time.deltaTime*speed;

		if (pos_z > z_length)
			pos_z = 0;

		Vector3 pos = transform.localPosition;
		pos.z = charactersDistance - pos_z;

		transform.localPosition = pos;


    }
}
