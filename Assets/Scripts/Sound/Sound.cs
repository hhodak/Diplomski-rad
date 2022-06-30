using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public AudioClip audioClip;
    public string name;
    public float volume;
    public float pitch;
    public float spatialBlend;
    public bool playOnAwake;
    public bool loop;
    [HideInInspector]
    public AudioSource audioSource;
}
