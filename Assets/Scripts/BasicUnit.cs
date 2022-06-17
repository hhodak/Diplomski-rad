using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit", menuName = "Create New Unit/Basic")]
public class BasicUnit : ScriptableObject
{
    public enum UnitType
    {
        Worker,
        Melee,
        Ranged,
        Siege,
        Spellcaster
    }

    [Header("Unit settings")]
    public string unitName;
    public GameObject cubePrefab;
    public GameObject spherePrefab;
    public GameObject icon;
    public UnitType unitType;
    public float spawnTime;

    [Header("Unit stats")]
    public UnitStatTypes.Base baseStats;
    public UnitStatTypes.Siege siegeStats;
}
