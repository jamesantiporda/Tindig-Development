using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Slider masterSlider, musicSlider, sfxSlider;
    public AudioMixer audioMixer;
    private float masterVolume, musicVolume, sfxVolume;
    public TMP_Text difficulty_text;

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
        int difficulty_int = PlayerPrefs.GetInt("Difficulty");
        switch (difficulty_int)
        {
            case 0:
                difficulty_text.text = "Easy";
                break;
            case 1:
                difficulty_text.text = "Medium";
                break;
            case 2:
                difficulty_text.text = "Hard";
                break;
            case 3:
                difficulty_text.text = "Expert";
                break;
            default:
                difficulty_text.text = "ERROR";
                break;
        }
    }
}
