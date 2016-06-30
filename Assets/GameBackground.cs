using UnityEngine;
using System.Collections;

public class GameBackground : MonoBehaviour {

    private CharactersManager charactersManager;
    public Renderer renderer;

    void Start()
    {
        charactersManager = Game.Instance.GetComponent<CharactersManager>();
        Data.Instance.events.OnChangeMood += OnChangeMood;
    }
    void OnDestroy()
    {
        Data.Instance.events.OnChangeMood -= OnChangeMood;
    }
    void OnChangeMood(int id)
    {
        string texture = Game.Instance.moodManager.GetMood(id).backgroundTexture;
        Material mat = Resources.Load("Materials/backgrounds/" + texture, typeof(Material)) as Material;
        renderer.material = mat;

    }
	void Update () {
        Vector3 pos = charactersManager.getPosition();
        pos.x = 0;
        pos.y = 0;
        transform.position = pos;
	}
}
