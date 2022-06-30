using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHandler : MonoBehaviour
{
    public static UnitHandler instance;
    //[SerializeField]
    public BasicUnit worker, melee, ranged, siege, spellcaster;

    public LayerMask playerUnitLayer, enemyUnitLayer;

    private void Awake()
    {
        instance = this;
    }

    public UnitStatTypes.Base GetBasicUnitStats(string type)
    {
        BasicUnit unit;
        switch (type)
        {
            case "Melee":
                unit = melee;
                break;
            case "Ranged":
                unit = ranged;
                break;
            case "Siege":
                unit = siege;
                break;
            case "Spellcaster":
                unit = spellcaster;
                break;
            case "Worker":
                unit = worker;
                break;
            default:
                Debug.Log($"Unit type {type} does not exist or it is not implemented yet!");
                return null;
        }
        return unit.baseStats;
    }
}

