using UnityEngine;
using System.Collections;

public class Floor : MonoBehaviour
{

    [SerializeField]
    GameObject[] areas;
    public int z_length;
    private bool isMoving;

    private CharactersManager charactersManager;

    public void Init(CharactersManager charactersManager)
    {
        isMoving = true;
        Data.Instance.events.OnGamePaused += OnGamePaused;
        Data.Instance.events.OnAvatarCrash += OnAvatarCrash;
        Data.Instance.events.OnAvatarFall += OnAvatarCrash;
        Data.Instance.events.OnChangeMood += OnChangeMood;
        this.charactersManager = charactersManager;
    }
    void OnDestroy()
    {
        Data.Instance.events.OnGamePaused -= OnGamePaused;
        Data.Instance.events.OnAvatarCrash -= OnAvatarCrash;
        Data.Instance.events.OnAvatarFall -= OnAvatarCrash;
        Data.Instance.events.OnChangeMood -= OnChangeMood;
    }
    void OnGamePaused(bool paused)
    {
        isMoving = !paused;
    }
    void OnChangeMood(int id)
    {
        print("OnChangeMood " + id);
        string texture = Game.Instance.moodManager.GetMood(id).floorTexture;
        
        foreach (GameObject area in areas)
        {
            Material mat = Resources.Load("Materials/Floors/" + texture, typeof(Material)) as Material;
            area.GetComponent<MeshRenderer>().material = mat;
        }

    }
    void OnAvatarCrash(CharacterBehavior cb)
    {
        isMoving = false;
    }
    void Update()
    {
        if (!isMoving) return;
        if (!charactersManager) return;

        //if (charactersManager.getDistance() > transform.localPosition.z+10)
        //{
        Vector3 pos = transform.localPosition;
        pos.z = charactersManager.getDistance();
        transform.localPosition = pos;
        foreach (GameObject area in areas)
        {
            pos = area.transform.localPosition;
            pos.z -= 0.05f;

            if (pos.z < -z_length)
                pos.z = z_length;

            area.transform.localPosition = pos;
        }


    }
}
