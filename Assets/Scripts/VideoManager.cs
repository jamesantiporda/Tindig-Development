using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    public VideoPlayer video;
    public float defaultVolume = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        video.SetDirectAudioVolume(0, defaultVolume + (PlayerPrefs.GetFloat("SFXVolume")/ 100) + (PlayerPrefs.GetFloat("MasterVolume") / 200));
    }

    // Update is called once per frame
    void Update()
    {
        video.SetDirectAudioVolume(0, defaultVolume + (PlayerPrefs.GetFloat("SFXVolume") / 100) + (PlayerPrefs.GetFloat("MasterVolume") / 200));
    }
}
