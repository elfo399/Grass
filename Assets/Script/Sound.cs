using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound {

    public enum AudioTypes {
        SOUNDEFFECT,
        MUSIC
    }
    
    [HideInInspector] public AudioSource source;
    public AudioClip clip;
    public string clipName;
    public bool isLoop;
    public bool playOnAwake;
    [Range(0, 1)] public float volume = 0.5f;
    public AudioTypes audioType;
}
