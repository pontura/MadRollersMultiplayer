using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

    public AudioClip interfaces;
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
		Data.Instance.GetComponent<Tracker> ().TrackScreen ("Main Menu");
		OnInterfacesStart ();

		Data.Instance.events.OnVersusTeamWon += OnVersusTeamWon;
        Data.Instance.events.StartMultiplayerRace += StartMultiplayerRace;
        Data.Instance.events.OnInterfacesStart += OnInterfacesStart;
      //  Data.Instance.events.OnAvatarChangeFX += OnAvatarChangeFX;
        Data.Instance.events.OnAvatarDie += OnAvatarDie;
        Data.Instance.events.OnGamePaused += OnGamePaused;
        Data.Instance.events.SetVolume += SetVolume;
        Data.Instance.events.OnAvatarCrash += OnAvatarCrash;
        Data.Instance.events.OnAvatarFall += OnAvatarCrash;
     //   Data.Instance.events.OnSoundFX += OnSoundFX;
        Data.Instance.events.OnListenerDispatcher += OnListenerDispatcher;
		Data.Instance.events.OnMusicStatus += OnMusicStatus;

		if (!Data.Instance.musicOn)
			audioSource.enabled = false;
    }
	void OnMusicStatus(bool isOn)
	{
		audioSource.enabled = isOn;
	}
	void OnVersusTeamWon(int teamID)
	{
		playSound( interfaces );
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
		if (audioSource.clip!=null && audioSource.clip.name == _clip.name) return;
        stopAllSounds();
        audioSource.clip = _clip;
        audioSource.Play();
        audioSource.loop = looped;
    }
    void OnGamePaused(bool paused)
    {
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
		PlayMainTheme ();
    }
	public void BossMusic(bool isBoss)
	{
//		return;
//		if (!isBoss)
//			PlayMainTheme ();
//		else
//			playSound(IndestructibleFX);
	}
//    void OnAvatarChangeFX(Player.fxStates state)
//    {
//		if (state == Player.fxStates.NORMAL)
//			PlayMainTheme ();
//        else
//            playSound(IndestructibleFX);
//    }
    void OnAvatarDie(CharacterBehavior player)
    {
        if (Game.Instance.GetComponent<CharactersManager>().getTotalCharacters() > 0) return;
        playSound(deathFX, false);
    }
    public void stopAllSounds()
    {
        audioSource.Stop();
		audioSource.clip = null;
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
	void PlayMainTheme()
	{
		string soundName = "song0";
		switch(Data.Instance.videogamesData.actualID)
		{
		case 0:
			soundName = "song0";
			break;
		case 1:
			soundName = "song1";
			break;
		case 2:
			soundName = "song2";
			break;
		}
		audioSource.clip = Resources.Load("songs/" + soundName) as AudioClip;
		audioSource.Play();
		audioSource.loop = true;
	}
}
