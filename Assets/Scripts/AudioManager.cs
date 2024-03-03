using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [Header("---------Audio Source---------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("---------Audio Clip-----------")]
    public AudioClip bgm;

    public AudioClip lightHit, mediumHit, heavyHit;

    public AudioClip lightWoosh, mediumWoosh, heavyWoosh;

    public AudioClip block, parry;

    public AudioClip menuOpen, menuClose;

    [Header("---------MC Clips-----------")]
    public AudioClip mcBalisong;
    public AudioClip mcFirecracker;
    public AudioClip mcFirecrackerPop;
    public AudioClip mcZipper;

    [Header("---------Footsteps---------")]
    public AudioClip normalFootstep0;
    public AudioClip normalFootstep1;
    public AudioClip sandFootstep0;
    public AudioClip sandFootstep1;
    public AudioClip sandFootstep2;
    public AudioClip sandFootstep3;
    public AudioClip dash;

    [SerializeField] AudioMixer mixer;

    public int groundType = 0;

    AudioClip[] audioClips;

    AudioClip[] normalFootsteps;
    AudioClip[] sandFootsteps;

    private void Start()
    {
        if(PlayerPrefs.HasKey("MasterVolume"))
        {
            SetMasterVolume(PlayerPrefs.GetFloat("MasterVolume"));
        }

        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            SetMusicVolume(PlayerPrefs.GetFloat("MusicVolume"));
        }

        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            SetSfxVolume(PlayerPrefs.GetFloat("SFXVolume"));
        }

        audioClips = new AudioClip[9];

        audioClips[0] = lightWoosh;
        audioClips[1] = mediumWoosh;
        audioClips[2] = heavyWoosh;
        audioClips[3] = lightHit;
        audioClips[4] = mcBalisong;
        audioClips[5] = mcFirecracker;
        audioClips[6] = mcFirecrackerPop;
        audioClips[7] = mediumHit;
        audioClips[8] = mcZipper;

        normalFootsteps = new AudioClip[2];

        normalFootsteps[0] = normalFootstep0;
        normalFootsteps[1] = normalFootstep1;

        sandFootsteps = new AudioClip[4];

        sandFootsteps[0] = sandFootstep0;
        sandFootsteps[1] = sandFootstep1;
        sandFootsteps[2] = sandFootstep2;
        sandFootsteps[3] = sandFootstep3;

        musicSource.clip = bgm;
        musicSource.Play();
    }

    public void PlaySFX(int audioID)
    {
        SFXSource.PlayOneShot(audioClips[audioID]);
    }

    public void PlayAudioClip(AudioClip audioClip)
    {
        SFXSource.PlayOneShot(audioClip);
    }

    public void PlayFootstep()
    {
        if(groundType == 0)
        {
            //int randomInt = Random.Range(0, 2);

            SFXSource.PlayOneShot(normalFootsteps[0]);
        }
        else if(groundType == 1)
        {
            int randomInt = Random.Range(0, 4);

            SFXSource.PlayOneShot(sandFootsteps[randomInt]);
        }
    }

    public void SetMasterVolume(float value)
    {
        Debug.Log("Master: " + value);
        SetVolume("MasterVolume", value);
        PlayerPrefs.SetFloat("MasterVolume", value);
    }

    public void SetMusicVolume(float value)
    {
        Debug.Log("Music: " + value);
        SetVolume("MusicVolume", value);
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public void SetSfxVolume(float value)
    {
        Debug.Log("SFX: " + value);
        SetVolume("SFXVolume", value);
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    public void SetVolume(string parameter, float value)
    {
        mixer.SetFloat(parameter, value);
    }

    //public void SliderChange(string type)
    //{
        //Debug.Log("Updated");
    //    if (type == "master")
    //    {
    //        SetMasterVolume(masterSlider.value);
    //    }
    //    else if (type == "music")
    //    {
    //        SetMusicVolume(musicSlider.value);
    //    }
    //    else if (type == "sfx")
    //    {
    //        SetSfxVolume(sfxSlider.value);
    //    }
    //}

    //private void Update()
    //{
    //    SetMasterVolume(masterSlider.value);
    //    SetMusicVolume(musicSlider.value);
    //    SetSfxVolume(sfxSlider.value);
    //}
}
