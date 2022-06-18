using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHandler : MonoBehaviour
{
    public static BuildingHandler instance;
    [SerializeField]
    BasicBuilding commandCenter, barracks, researchFacility, upgradeFacility;

    private void Awake()
    {
        instance = this;
    }

    public BuildingStatTypes.Base GetBasicBuildingStats(string type)
    {
        BasicBuilding building;
        switch (type)
        {
            case "CommandCenter":
                building = commandCenter;
                break;
            case "Barracks":
                building = barracks;
                break;
            case "ResearchFacility":
                building = researchFacility;
                break;
            case "UpgradeFacility":
                building = upgradeFacility;
                break;
            default:
                Debug.Log($"Unit type {type} does not exist or it is not implemented yet!");
                return null;
        }
        return building.baseStats;
    }
}
