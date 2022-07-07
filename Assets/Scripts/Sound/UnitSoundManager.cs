using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class UnitSoundManager : MonoBehaviour
{
    public Sound[] sounds;
    public AudioMixerGroup audioMixerGroup;


    // Start is called before the first frame update
    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.audioSource = gameObject.AddComponent<AudioSource>();
            s.audioSource.outputAudioMixerGroup = audioMixerGroup;
            s.audioSource.clip = s.audioClip;
            s.audioSource.volume = s.volume;
            s.audioSource.pitch = s.pitch;
            s.audioSource.spatialBlend = s.spatialBlend;
            s.audioSource.loop = s.loop;
            s.audioSource.playOnAwake = s.playOnAwake;
        }
    }

    public void PlaySound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        if (name == "Death")
        {
            transform.SetParent(null);
            StartCoroutine(DestroyGO());
        }
        s.audioSource.Play();
    }

    IEnumerator DestroyGO()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
