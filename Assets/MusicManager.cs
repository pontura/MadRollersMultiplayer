using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

    public AudioClip interfaces;
    public AudioClip MainTheme;
    public AudioClip IndestructibleFX;
    public AudioClip heartClip;
    public AudioClip consumeHearts;
    public AudioClip deathFX;
    public AudioClip enemyShout;
    public AudioClip enemyDead;
    public AudioClip credits;

    private float heartsDelay = 0.1f;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        GetComponent<AudioLowPassFilter>().enabled = false;
    }
	public void Init () {
        Data.Instance.events.StartMultiplayerRace += StartMultiplayerRace;
        Data.Instance.events.OnMissionStart += OnMissionStart;
        Data.Instance.events.OnInterfacesStart += OnInterfacesStart;
        Data.Instance.events.OnAvatarChangeFX += OnAvatarChangeFX;
        Data.Instance.events.OnAvatarDie += OnAvatarDie;
        Data.Instance.events.OnGamePaused += OnGamePaused;
        Data.Instance.events.SetVolume += SetVolume;
        Data.Instance.events.OnAvatarCrash += OnAvatarCrash;
        Data.Instance.events.OnAvatarFall += OnAvatarCrash;
     //   Data.Instance.events.OnSoundFX += OnSoundFX;
        Data.Instance.events.OnListenerDispatcher += OnListenerDispatcher;
	}
    void OnListenerDispatcher(string type)
    {
        
        if (type == "LevelFinish_hard" || type == "LevelFinish_medium" || type == "LevelFinish_easy")
        {
            GetComponent<AudioLowPassFilter>().enabled = true;
            Invoke("ResetFilter", 4.7f);
        }
    }
    
    void ResetFilter()
    {
        GetComponent<AudioLowPassFilter>().enabled = false;
    }
    //void OnSoundFX(string name)
    //{
    //    switch (name)
    //    {
    //        case "enemyShout": audioSource.PlayOneShot(enemyShout); break;
    //        case "enemyDead": audioSource.PlayOneShot(enemyDead); break;
    //        case "consumeHearts": audioSource.PlayOneShot(consumeHearts); break;
    //    }
    //}
    void OnAvatarCrash(CharacterBehavior cb)
    {
        if (Game.Instance.GetComponent<CharactersManager>().getTotalCharacters() > 0) return;
        audioSource.Stop();
    }
    public void SetVolume(float vol)
    {
        audioSource.volume = vol;
    }
    public void playSound(AudioClip _clip, bool looped = true)
    {        
        if (audioSource.clip.name == _clip.name) return;
        stopAllSounds();
        audioSource.clip = _clip;
        audioSource.Play();
        audioSource.loop = looped;
    }
    void OnGamePaused(bool paused)
    {
        print("OnGamePaused" + paused);
        if(paused)
            audioSource.Stop();
        else
            audioSource.Play();
    }
    void OnInterfacesStart()
    {
        playSound( interfaces );
    }
    void StartMultiplayerRace()
    {
        playSound(MainTheme);
    }
    void OnMissionStart(int id)
    {
        //playSound(MainTheme);
	}
    void OnAvatarChangeFX(Player.fxStates state)
    {
        if (state == Player.fxStates.NORMAL)
            playSound(MainTheme);
        else
            playSound(IndestructibleFX);
    }
    void OnAvatarDie(CharacterBehavior player)
    {
        if (Game.Instance.GetComponent<CharactersManager>().getTotalCharacters() > 0) return;
        playSound(deathFX, false);
    }
    public void stopAllSounds()
    {
        audioSource.Stop();
    }

    float nextHeartSoundTime;
    public void addHeartSound()
    {
        if (Time.time >= nextHeartSoundTime)
        {
          audioSource.PlayOneShot(heartClip);
          nextHeartSoundTime = Time.time + heartsDelay;
          if (Random.Range(0, 500) > 490)
          {
              //Data.Instance.voicesManager.ComiendoCorazones();
          }
        }
    }
}
