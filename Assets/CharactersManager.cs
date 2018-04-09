﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharactersManager : MonoBehaviour {

    public CharacterBehavior character;
  //  public EnergyBar energyBar;
    public List<CharacterBehavior> characters;
    public List<CharacterBehavior> deadCharacters;

    private Vector3 characterPosition = new Vector3(0,0,0);

    private float separationX  = 2;

    public float distance;
    private float speedRun = 19;
    private Missions missions;
    public List<int> playerPositions;
    private bool gameOver;
    private IEnumerator RalentaCoroutine;
    bool isArcadeMultiplayer;

    void Awake()
    {
        distance = 20;
    }
    void Start()
    {
        missions = Data.Instance.GetComponent<Missions>();
        isArcadeMultiplayer = Data.Instance.isArcadeMultiplayer;
		//if (!isArcadeMultiplayer) {			
			//StartCoroutine (RalentaCoroutine);
		//}
		if (!Data.Instance.isReplay) {
			RalentaCoroutine = DoRalentaCoroutine (0.5f, 0, 0.1f);
			StartCoroutine (RalentaCoroutine);
		}
    }
    void OnListenerDispatcher(string type)
    {
        if (type == "Ralenta")
        {
            RalentaCoroutine = DoRalentaCoroutine(4, 1f, 0.05f);
            StartCoroutine(RalentaCoroutine);
        }
        if (type == "BonusEntrande")
        {
            RalentaCoroutine = DoRalentaCoroutine(11, 0, 0.05f);
            StartCoroutine(RalentaCoroutine);

            Data.Instance.events.OnCreateBonusArea();
            foreach (CharacterBehavior cb in characters)
                cb.transform.localPosition = new Vector3(0, 25, 0);
        }
    }
    void StartMultiplayerRace()
    {
		if (Data.Instance.isReplay) {
			// no hace nada:
		} else {
			RalentaCoroutine = DoRalentaCoroutine (2, 0, 0.05f);
			StartCoroutine (RalentaCoroutine);
		}
    }
    IEnumerator DoRalentaCoroutine(float _speedRun, float delay, float speedTeRecover)
    {
        yield return new WaitForSeconds(delay);
        if (delay > 0)
        {
            while (speedRun > _speedRun)
            {
                yield return new WaitForEndOfFrame();
                speedRun -= Time.deltaTime + speedTeRecover;
            }
        }
        speedRun = _speedRun;
        while (speedRun < 19)
        {
            yield return new WaitForEndOfFrame();
            speedRun += Time.deltaTime + speedTeRecover;
        }
        speedRun = 19;
        yield return null;
    }
    void Update()
    {
		
		if (Data.Instance.isArcadeMultiplayer) return;
		if (Game.Instance.level.waitingToStart) return;
        if (gameOver) return;
        distance += speedRun * Time.deltaTime;

        if(!isArcadeMultiplayer)
            missions.updateDistance(distance);

       // lastDistance = distance;
    }
    public int GetPositionByID(int _playerID)
    {
		if (distance < 100) return 0;
		if (Data.Instance.isArcadeMultiplayer && Game.Instance.level.waitingToStart) return 0;
        int position = 0;
        foreach(int playerID in playerPositions)
        {
            if(playerID == _playerID)
                return position;
            position++;
        }
        return position;
    }
    public void Init()
    {
        Data.Instance.events.OnAlignAllCharacters += OnAlignAllCharacters;
        Data.Instance.events.OnListenerDispatcher += OnListenerDispatcher;
        Data.Instance.events.OnReorderAvatarsByPosition += OnReorderAvatarsByPosition;
        Data.Instance.events.OnAvatarCrash += OnAvatarCrash;
        Data.Instance.events.OnAvatarFall += OnAvatarFall;
        Data.Instance.events.StartMultiplayerRace += StartMultiplayerRace;

        Vector3 pos;

		float _y = 1;

		if (Data.Instance.isReplay)
			_y = 15;
		
		pos = new Vector3(1, _y, 1);

		//if (Data.Instance.playMode == Data.PlayModes.STORY) {
		//	addCharacter(pos, 0); playerPositions.Add(0);
		//}
        if (Data.Instance.multiplayerData.player1) { addCharacter(pos, 0); playerPositions.Add(0); };
        if (Data.Instance.multiplayerData.player2) { addCharacter(pos, 1); playerPositions.Add(1); };
        if (Data.Instance.multiplayerData.player3) { addCharacter(pos, 2); playerPositions.Add(2); };
        if (Data.Instance.multiplayerData.player4) { addCharacter(pos, 3); playerPositions.Add(3); };
    }
    void OnDestroy()
    {
        Data.Instance.events.OnAvatarCrash -= OnAvatarCrash;
        Data.Instance.events.OnAvatarFall -= OnAvatarFall;
        Data.Instance.events.OnReorderAvatarsByPosition -= OnReorderAvatarsByPosition;
        Data.Instance.events.OnListenerDispatcher -= OnListenerDispatcher;
        Data.Instance.events.StartMultiplayerRace -= StartMultiplayerRace;
        Data.Instance.events.OnAlignAllCharacters -= OnAlignAllCharacters;
    }
    void OnReorderAvatarsByPosition(List<int> playerPositions)
    {
        this.playerPositions = playerPositions;
    }
    void OnAvatarFall(CharacterBehavior characterBehavior)
    {
        killCharacter(characterBehavior);
    }
    void OnAvatarCrash(CharacterBehavior characterBehavior)
    {
        killCharacter(characterBehavior);
    }
    public bool existsPlayer(int id)
    {
        bool exists = false;
        characters.ForEach((cb) =>
        {
            if (cb.player.id == id) exists = true;
        });
        return exists;
    }
    public void addNewCharacter(int id)
    {
		if (characters.Count == 0)
			return;
		
        Data.Instance.events.OnSoundFX("coin", id);
        Data.Instance.events.OnAddNewPlayer(id);

        Vector3 pos = characters[0].transform.position;
        pos.y += 3;
        pos.x = 0;
        addCharacter(pos, id);
    }
    public void addCharacter(Vector3 pos, int id)
    {
		float _x;
		if (Data.Instance.isReplay)
			_x = 0;
		else
			_x = (3.5f * id) - (5.3f);
			
		pos = new Vector3(_x,pos.y);

        CharacterBehavior newCharacter = null;
        foreach (CharacterBehavior cb in deadCharacters)
        {
            if (cb.player.id == id)
            {
                newCharacter = cb;
            }
        }
        if (newCharacter == null)
            newCharacter = Instantiate(character, Vector3.zero, Quaternion.identity) as CharacterBehavior;
        else
            deadCharacters.Remove(newCharacter);

        newCharacter.Revive();

        newCharacter.GetComponent<Player>().Init(id);

        newCharacter.GetComponent<Player>().id = id;

        characters.Add(newCharacter);
        newCharacter.transform.position = pos;
    }
    public void killCharacter(CharacterBehavior characterBehavior)
    {
        characters.ForEach((cb) =>
        {
            if (cb.player.id == characterBehavior.player.id)
            {
                characters.Remove(cb);
                deadCharacters.Add(cb);
                if (characters.Count == 0)
                {
                    StartCoroutine(restart(cb));
                }
            }
        });
        Data.Instance.events.OnAvatarDie(characterBehavior);
    }
    IEnumerator restart(CharacterBehavior cb)
    {
        gameOver = true;
        yield return new WaitForSeconds(0.05f);
        Data.Instance.events.OnGameOver();
        yield return new WaitForSeconds(1.32f);
    }
    public CharacterBehavior getMainCharacter()
    {
        if (characters.Count <= 0)
        {
            print("[ERROR] No hay más characters y sigue pidiendo...");
            return null;
        }
        return characters[0];
    }
    public Vector3 getPositionMainCharacter()
    {
        return getMainCharacter().transform.position;
    }
    public Vector3 getPosition()
    {
        if (characters.Count > 1)
        {
            Vector3 normalPosition = Vector3.zero;
            Vector3 lastCharacterPosition = Vector3.zero;
            float MaxDistance = 0;
            foreach(CharacterBehavior cb in characters)
            {
                if(lastCharacterPosition != Vector3.zero)
                {
                    float dist = Vector3.Distance(cb.transform.localPosition, lastCharacterPosition);
                    if(dist>MaxDistance) MaxDistance = dist;
                }
                lastCharacterPosition = cb.transform.localPosition;
                normalPosition += lastCharacterPosition;
            }

            normalPosition /= characters.Count;
            normalPosition.y += 0.15f + (MaxDistance / 4f );
            normalPosition.z -= 0.2f + (MaxDistance/26);

            return normalPosition;
        }
        else if (characters.Count == 0) return characterPosition;
        else
            characterPosition = characters[0].transform.position;

        return characterPosition;
    }
    public int getTotalCharacters()
    {
        return characters.Count;
    }
    public float getDistance()
    {
        return distance;
    }
    void OnAlignAllCharacters()
    {
        foreach (CharacterBehavior cb in characters)
        {
            Vector3 pos = cb.transform.localPosition;
            pos.x = 0;
            pos.y = 1;
            cb.transform.localPosition = pos;
        }
    }
	public void ResetJumps(){
		foreach (CharacterBehavior cb in characters) {
			cb.ResetJump ();
		}
	}
	public void OnLevelComplete()
	{
		foreach (CharacterBehavior cb in characters) {
			//cb.SuperJump (2200);
			cb.player.SetInvensible ();
		}
	}
}
