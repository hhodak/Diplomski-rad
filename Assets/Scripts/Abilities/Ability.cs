using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    public BasicAbility abilityType;
    public AbilityStatTypes.Base baseStats;
    public AbilityStatTypes.Healing healingStats;
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        baseStats = abilityType.baseStats;
        healingStats = abilityType.healingStats;
    }

    public void HealTarget(Transform target)
    {
        UnitStatDisplay unit = target.gameObject.GetComponentInChildren<UnitStatDisplay>();
        unit.RestoreHealth(abilityType.healingStats.healingAmount);
        ShowHealingAnimation();
        PlaySound();
        StartCoroutine(DestroyAbility());
    }

    void ShowHealingAnimation()
    {
        Transform healingParticles = transform.GetChild(0);
        healingParticles.gameObject.SetActive(true);
    }

    IEnumerator DestroyAbility()
    {
        yield return new WaitForSeconds(baseStats.duration);
        Destroy(gameObject);
    }

    void PlaySound()
    {
        audioSource.Play();
    }
}
