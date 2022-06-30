using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void MeleeAttack()
    {
        PlaySound();
    }

    void PlaySound()
    {
        audioSource.Play();
    }
}
