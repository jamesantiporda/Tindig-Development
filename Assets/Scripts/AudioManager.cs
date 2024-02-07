using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("---------Audio Source---------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("---------Audio Clip-----------")]
    public AudioClip bgm;

    public AudioClip lightHit, mediumHit, heavyHit;

    public AudioClip lightWoosh, mediumWoosh, heavyWoosh;

    [Header("---------MC Clips-----------")]
    public AudioClip mcBalisong;
    public AudioClip mcFirecracker;
    public AudioClip mcFirecrackerPop;
    public AudioClip mcZipper;

    AudioClip[] audioClips;

    private void Start()
    {
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
}
