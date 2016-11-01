using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharactersManager : MonoBehaviour {

    public CharacterBehavior character;
  //  public EnergyBar energyBar;
    public List<CharacterBehavior> characters;
    public List<CharacterBehavior> deadCharacters;

    public Color[] colors;

    private Vector3 characterPosition = new Vector3(0,0,0);

    private float separationX  = 2;

    public float distance;
    private float speedRun = 19;
    private Missions missions;
    public List<int> playerPositions;
    private bool gameOver;
    private IEnumerator RalentaCoroutine;

    void Awake()
    {
        colors = Data.Instance.multiplayerData.colors;
        distance = 20;
    }
    void Start()
    {
        missions = Data.Instance.GetComponent<Missions>();
        
    }
    void OnListenerDispatcher(string type)
    {
        if (type == "Ralenta")
        {
            RalentaCoroutine = DoRalentaCoroutine(11);
            StartCoroutine(RalentaCoroutine);
        }
        if (type == "BonusEntrande")
        {
            RalentaCoroutine = DoRalentaCoroutine(11);
            StartCoroutine(RalentaCoroutine);

            Data.Instance.events.OnCreateBonusArea();
            foreach (CharacterBehavior cb in characters)
                cb.transform.localPosition = new Vector3(0, 25, 0);
        }
    }
    void StartMultiplayerRace()
    {
        RalentaCoroutine = DoRalentaCoroutine(2);
        StartCoroutine(RalentaCoroutine);
    }
    IEnumerator DoRalentaCoroutine(float _speedRun)
    {
        float speedTeRecover = 0.05f;
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
        if (Game.Instance.level.waitingToStart) return;
        if (gameOver) return;
        distance += speedRun * Time.deltaTime;
        missions.updateDistance(distance);
       // lastDistance = distance;
    }
    public int GetPositionByID(int _playerID)
    {
        if (Game.Instance.level.waitingToStart) return 0;
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
        Data.Instance.events.OnListenerDispatcher += OnListenerDispatcher;
        Data.Instance.events.OnReorderAvatarsByPosition += OnReorderAvatarsByPosition;
        Data.Instance.events.OnAvatarCrash += OnAvatarCrash;
        Data.Instance.events.OnAvatarFall += OnAvatarFall;
        Data.Instance.events.StartMultiplayerRace += StartMultiplayerRace;

        Vector3 pos;
        pos = new Vector3(1, 10, 1);

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
        print("addNewCharacter ");
        Data.Instance.events.OnSoundFX("coin", id);
        Data.Instance.events.OnAddNewPlayer(id);
        Vector3 pos = characters[0].transform.position;
        pos.y += 3;
        pos.x = 0;
        addCharacter(pos, id);
    }
    public void addCharacter(Vector3 pos, int id)
    {
        if (Game.Instance.level.waitingToStart)
        {
            pos = new Vector3((3.5f * id) - (4.5f), 1);
        }
        else
        {
            pos.x *= separationX;
        }
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
        print(" killCharacter " + characterBehavior.player.id);
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
       // Destroy(cb.GetComponent<Player>().energyBar.gameObject);
        //Destroy(cb.gameObject);
        //Game.Instance.ResetLevel();
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
            float _x = 0;
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
            normalPosition.z -= 1.6f + (MaxDistance/17);

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
}
