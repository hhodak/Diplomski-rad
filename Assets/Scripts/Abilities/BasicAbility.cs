using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Create New Ability/Basic")]

public class BasicAbility : ScriptableObject
{
    public enum AbilityType
    {
        Healing,
        TargetDamage,
        SplashDamage,
        Stun
    }

    [Header("Ability settings")]
    public string abilityName;
    public GameObject abilityPrefab;
    public GameObject icon;
    public AbilityType abilityType;

    [Header("Ability stats")]
    public AbilityStatTypes.Base baseStats;
    public AbilityStatTypes.Healing healingStats;
}
