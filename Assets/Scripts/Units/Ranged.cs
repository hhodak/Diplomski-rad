using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranged : MonoBehaviour
{
    public GameObject projectilePrefab;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void RangedAttack(Transform target, float damage)
    {
        GameObject newProjectile = Instantiate(projectilePrefab, transform.position, transform.rotation, transform);
        Projectile projectile = newProjectile.GetComponent<Projectile>();
        PlaySound();
        projectile.Launch(target.position, Projectile.ProjectileType.Arrow, damage);
    }

    void PlaySound()
    {
        audioSource.Play();
    }
}
