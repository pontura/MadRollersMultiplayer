using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class VoicesManager : MonoBehaviour
{
	public List<VoiceData> tutorials;
	public List<VoiceData> intros;
	public List<VoiceData> welcome;
	public List<VoiceData> missionComplete;

	public List<VoiceData> lose_bad;
	public List<VoiceData> lose_good;
	public List<VoiceData> lose_great;

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
		audioSource.enabled = Data.Instance.voicesOn;

		if (!Data.Instance.voicesOn)
			return;
		
        Data.Instance.events.OnMissionComplete += OnMissionComplete;
        Data.Instance.events.OnListenerDispatcher += OnListenerDispatcher;
        Data.Instance.events.OnAvatarCrash += OnAvatarCrash;
        Data.Instance.events.OnAvatarFall += OnAvatarFall;
        Data.Instance.events.OnAvatarChangeFX += OnAvatarChangeFX;
        Data.Instance.events.SetVolume += SetVolume;
        Data.Instance.events.VoiceFromResources += VoiceFromResources; 
		Data.Instance.events.OnVoicesStatus += OnVoicesStatus;

		if (!Data.Instance.voicesOn)
			audioSource.enabled = false;
	}
	void OnVoicesStatus(bool isOn)
	{
		audioSource.enabled = isOn;
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
		Dead ();
    }
    private void OnAvatarFall(CharacterBehavior cb)
    {
		Dead ();
    }
	void Dead()
	{
		float distance = Game.Instance.level.charactersManager.distance;
		if (distance < 100)
			PlayRandom (lose_bad);
		else if (distance < 1000)
			PlayRandom (lose_good);
		else
			PlayRandom (lose_great);
	}
    private void OnAvatarChangeFX(Player.fxStates state)
    {
    }
    private void OnListenerDispatcher(string message)
    {
		if (Data.Instance.playMode == Data.PlayModes.COMPETITION) return;
        if (message == "ShowMissionId")
        {
			
        }
		else if (message == "ShowMissionName")
		{
			if(Data.Instance.missions.MissionActive.voices.Count > 0)
				PlaySequence (Data.Instance.missions.MissionActive.voices);
		}
    }
	int sequenceID = 0;
	bool onSequence = false;
	List<VoiceData> sequenceSaying;
	public void PlaySequence( List<VoiceData> clips)
	{
		if (clips.Count == 0)
			return;
		sequenceID = 0;
		talking = false;
		audioSource.Stop ();
		this.sequenceSaying = clips;
		onSequence = true;
		PlayNextSequencedClip ();
	}
	void PlayNextSequencedClip()
	{
		VoiceData newAudio = sequenceSaying[sequenceID];
		print (onSequence + " " + newAudio.audioClip + " " + sequenceID + "    count: " + sequenceSaying.Count);
		PlayClip(newAudio.audioClip); 
		sequenceID++;
		if (sequenceSaying.Count == sequenceID)
		{
			onSequence = false;
			Done ();
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
        audioSource.clip = audioClip;
        audioSource.Play();
		Data.Instance.events.OnTalk (true);
    }
	float timer;
	void Update()
	{
		if (!talking)
			return;
		
		if (audioSource.clip != null && audioSource.clip.length>0.1f && audioSource.time >= (audioSource.clip.length-0.02f)) {
			Done ();
		}
	}
	void Done()
	{
		if (onSequence)
			PlayNextSequencedClip ();
		else {
			talking = false;			
			Data.Instance.events.OnTalk (false);
		}
	}
}
