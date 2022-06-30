using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public Transform playerUnits;
    public Transform playerBuildings;

    private void Awake()
    {
        instance = this;
        SetBasicStats(playerUnits);
        SetBasicStats(playerBuildings);
    }

    void Update()
    {
        InputHandler.instance.HandleUserInput();
    }

    public void SetBasicStats(Transform type)
    {
        foreach (Transform child in type)
        {
            foreach (Transform obj in child)
            {
                if (type == playerUnits)
                {
                    PlayerUnit playerUnit = obj.GetComponent<PlayerUnit>();
                    playerUnit.baseStats = UnitHandler.instance.GetBasicUnitStats(AssetNameParser(child.name));
                }
                else if (type == playerBuildings)
                {
                    PlayerBuilding playerBuilding = obj.GetComponent<PlayerBuilding>();
                    playerBuilding.baseStats = BuildingHandler.instance.GetBasicBuildingStats(AssetNameParser(child.name));
                }
            }
        }
    }

    string AssetNameParser(string assetName)
    {
        if (assetName.Contains("Player"))
        {
            return assetName.Replace("Player", string.Empty);
        }
        else if (assetName.Contains("Enemy"))
        {
            return assetName.Replace("Enemy", string.Empty);
        }
        else
        {
            return assetName;
        }
    }
}
