using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    public BasicAbility abilityType;
    public AbilityStatTypes.Base baseStats;
    public AbilityStatTypes.Healing healingStats;

    void Start()
    {
        baseStats = abilityType.baseStats;
        healingStats = abilityType.healingStats;
    }

    public void HealTarget(Transform target)
    {
        UnitStatDisplay unit = target.gameObject.GetComponentInChildren<UnitStatDisplay>();
        unit.RestoreHealth(abilityType.healingStats.healingAmount);
        ShowHealingAnimation();
        DestroyAbility();
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
}
