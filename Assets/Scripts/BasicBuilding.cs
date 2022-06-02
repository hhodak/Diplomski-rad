using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "Create new building/Basic")]
public class BasicBuilding : ScriptableObject
{
    public enum BuildingType
    {
        CommandCenter,
        Barracks,
        ResearchFacility,
        UpgradeFacility
    }

    [Header("Building Settings")]
    public BuildingType buildingType;
    public string buildingName;
    public GameObject buildingPrefab;
    public GameObject buildingBlueprintPrefab;
    public GameObject icon;
    public float spawnTime;
    public Vector3 spawnLocation;

    [Header("Building Base Stats")]
    public BuildingStatTypes.Base baseStats;
}