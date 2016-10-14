using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource;
    private AudioSource loopAudioSource;
    public float volume;
    public string[] FXCheer;
    public string[] FXJump;
    public string[] FXCrash;

    void Start()
    {
        FXCheer = new string[] { "FX vox iuju", "FX vox risa", "FX vox uoo", "FX vox uoo", "FX vox yepa" };
        FXJump = new string[] { "FX jump00", "FX jump02" };
        FXCrash = new string[] { "FX vox muerte01", "FX vox muerte02" };

        OnSoundsVolumeChanged(volume);

        Data.Instance.events.OnSoundFX += OnSoundFX;
       // Events.OnSoundFXLoop += OnSoundFXLoop;
       // Events.OnSoundsVolumeChanged += OnSoundsVolumeChanged;        
        //Events.OnHeroDie += OnHeroDie;
    }
    void OnHeroDie()
    {
        OnSoundFXLoop("");
    }
    void OnDestroy()
    {
        Data.Instance.events.OnSoundFX -= OnSoundFX;
       // Events.OnSoundFXLoop -= OnSoundFXLoop;
       // Events.OnSoundsVolumeChanged -= OnSoundsVolumeChanged;
       // Events.OnHeroDie -= OnHeroDie;
        if (loopAudioSource)
        {
            loopAudioSource = null;
            loopAudioSource.Stop();
        }
    }
    void OnSoundsVolumeChanged(float value)
    {
        audioSource.volume = value;
        volume = value;

        if (value == 0 || value == 1)
            PlayerPrefs.SetFloat("SFXVol", value);
    }
    void OnSoundFXLoop(string soundName)
    {
        if (volume == 0) return;

        if (!loopAudioSource)
            loopAudioSource = gameObject.AddComponent<AudioSource>() as AudioSource;

        if (soundName != "")
        {
            loopAudioSource.clip = Resources.Load("Sound/" + soundName) as AudioClip;
            loopAudioSource.Play();
            loopAudioSource.loop = true;
        }
        else
        {
            loopAudioSource.Stop();
        }
    }
    void OnSoundFX(string soundName, int playerID)
    {
        if (soundName == "")
        {
            audioSource.Stop();
            return;
        }
        else if (soundName == "FXCheer")
            soundName = GetRandomSound(FXCheer);
        else if (soundName == "FXJump")
            soundName = GetRandomSound(FXJump);
        else if(soundName == "FXCrash")
            soundName = GetRandomSound(FXCrash);

        if (volume == 0) return;

        if (playerID == 0)
            audioSource.panStereo = -0.8f;
        else if (playerID == 1)
            audioSource.panStereo = -0.3f;
        else if (playerID == 2)
            audioSource.panStereo = 0.3f;
        else if (playerID == 4)
            audioSource.panStereo = 0.8f;
        else
            audioSource.panStereo = 0;

        audioSource.PlayOneShot(Resources.Load("Sound/" + soundName) as AudioClip);
    }
    private string GetRandomSound(string[] arr)
    {
        return arr[Random.Range(0, arr.Length-1)];
    }
}
