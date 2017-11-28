﻿using UnityEngine;
using System.Collections;

public class GameBackground : MonoBehaviour {

    private CharactersManager charactersManager;
    public Renderer renderer;
    public Material[] materials;
    public int id;

    void Start()
    {
        id = 0;
        charactersManager = Game.Instance.GetComponent<CharactersManager>();
        Data.Instance.events.OnListenerDispatcher += OnListenerDispatcher;
        Data.Instance.events.OnChangeMood += OnChangeMood;
    }
    void OnDestroy()
    {
        Data.Instance.events.OnChangeMood -= OnChangeMood;
        Data.Instance.events.OnListenerDispatcher -= OnListenerDispatcher;
    }
    void OnListenerDispatcher(string type)
    {
        print("__________OnListenerDispatcher " + type);
        if (type == "LevelFinish" 
            || type == "LevelFinish_medium"
            || type == "LevelFinish_easy"
            || type == "LevelFinish_hard")
        {
            
            id++;
            if (id > materials.Length - 1) id = 0;
            renderer.material = materials[id];
            print("moood id:  " + id);
        }
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
