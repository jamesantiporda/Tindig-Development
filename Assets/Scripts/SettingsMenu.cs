using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Slider masterSlider, musicSlider, sfxSlider;
    public AudioMixer audioMixer;
    private float masterVolume, musicVolume, sfxVolume;

    // Start is called before the first frame update
    void Start()
    {
        audioMixer.GetFloat("MasterVolume", out masterVolume);
        masterSlider.value = masterVolume;
        audioMixer.GetFloat("MusicVolume", out musicVolume);
        musicSlider.value = musicVolume;
        audioMixer.GetFloat("SFXVolume", out sfxVolume);
        sfxSlider.value = sfxVolume;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
