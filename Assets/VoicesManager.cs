using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class VoicesManager : MonoBehaviour
{
	
	public List<VoiceData> intros;
	public List<VoiceData> welcome;
	public VoiceData selectMadRollers;

	public AudioSpectrum audioSpectrum;
	[Serializable]
	public class VoiceData
	{
		public string text;
		public AudioClip audioClip;
	}
	public AudioSource audioSource;

    public void Init()
    {
		Data.Instance.events.OnGameStart += OnGameStart;
        Data.Instance.events.OnMissionComplete += OnMissionComplete;
        Data.Instance.events.OnListenerDispatcher += OnListenerDispatcher;
        Data.Instance.events.OnAvatarCrash += OnAvatarCrash;
        Data.Instance.events.OnAvatarFall += OnAvatarFall;
        Data.Instance.events.OnAvatarChangeFX += OnAvatarChangeFX;
        Data.Instance.events.SetVolume += SetVolume;
        Data.Instance.events.VoiceFromResources += VoiceFromResources;        
    }
	void OnGameStart()
	{
		
	}
    void SetVolume(float vol)
    {
        audioSource.volume = vol;
    }
    private void OnMissionComplete(int id)
    {
    }
    private void OnAvatarCrash(CharacterBehavior cb)
    {
    }
    private void OnAvatarFall(CharacterBehavior cb)
    {
    }
    private void OnAvatarChangeFX(Player.fxStates state)
    {
    }
    private void OnListenerDispatcher(string message)
    {
        if (Data.Instance.playMode == Data.PlayModes.COMPETITION) return;
        if (message == "ShowMissionId")
        {
         	//PlayRandom("");
        }
        else if (message == "ShowMissionName")
		{
          //  PlayMission();
		}
    }
	public void PlayRandom( List<VoiceData> clips)
    {
		int rand = UnityEngine.Random.Range(0, clips.Count);
		PlayClip(clips[rand].audioClip); 
    }
    public void ComiendoCorazones()
    {
    }
    public void VoiceSecondaryFromResources(string name)
    {
    }
    public void VoiceFromResources(string name)
    {
    }
	bool talking;
	public void PlayClip(AudioClip audioClip)
    {
		talking = true;
		audioSpectrum.SetOn ();
		timer = 0;
        audioSource.clip = audioClip;
        audioSource.Play();
		Data.Instance.events.OnTalk (true);
    }
	float timer;
	void Update()
	{
		if (!talking)
			return;
		
		timer += Time.deltaTime;
		if (audioSource.clip != null && audioSource.clip.length>0 && timer > audioSource.clip.length && audioSource.isPlaying) {
			talking = false;			
			Data.Instance.events.OnTalk (false);
		}
	}
}
