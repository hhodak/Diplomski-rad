using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    public Transform enemyUnits;
    public Transform enemyBuildings;

    [SerializeField]
    private int numberOfUnits;
    [SerializeField]
    private int numberOfWorkers;
    public float numberOfResources;
    [SerializeField]
    private List<Transform> setResourceNodes;
    private static List<Transform> resourceNodes;
    public static bool isRemovingResourceNode = false;

    int unitSpawningCount = 0;
    [SerializeField]
    private Vector3 approxEnemyBase;


    private void Awake()
    {
        instance = this;
        resourceNodes = setResourceNodes;
        SetBasicStats(enemyUnits);
        SetBasicStats(enemyBuildings);
    }

    private void Start()
    {
        GatherResources();
    }

    private void Update()
    {
        CheckUnitNumber();
        SpawnUnits();
        if (CanAttackPlayer())
        {
            AttackPlayer();
        }
    }

    public void SetBasicStats(Transform type)
    {
        foreach (Transform child in type)
        {
            foreach (Transform obj in child)
            {
                if (type == enemyUnits)
                {
                    EnemyUnit enemyUnit = obj.GetComponent<EnemyUnit>();
                    enemyUnit.baseStats = UnitHandler.instance.GetBasicUnitStats(AssetNameParser(child.name));

                }
                else if (type == enemyBuildings)
                {
                    EnemyBuilding enemyBuilding = obj.GetComponent<EnemyBuilding>();
                    enemyBuilding.baseStats = BuildingHandler.instance.GetBasicBuildingStats(AssetNameParser(child.name));
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

    void GatherResources()
    {
        Transform enemyWorkers = enemyUnits.GetChild(0);
        foreach (Transform worker in enemyWorkers)
        {
            Worker w = worker.GetComponent<Worker>();
            w.HandleWorking(true, resourceNodes[0]);
        }
        isRemovingResourceNode = false;
    }

    void SpawnUnits()
    {
        if (numberOfWorkers < 5)
        {
            Transform enemyCommandCenters = enemyBuildings.GetChild(0);
            foreach (Transform commandCenter in enemyCommandCenters)
            {
                if (numberOfResources >= UnitHandler.instance.worker.baseStats.cost)
                {
                    commandCenter.GetComponent<EnemyBuilding>().SpawnUnit(UnitHandler.instance.worker);
                }
            }
        }

        if (numberOfUnits < 10)
        {
            Transform enemyBarracks = enemyBuildings.GetChild(1);
            foreach (Transform barracks in enemyBarracks)
            {
                switch (unitSpawningCount % 2)
                {
                    case 0:
                        if (numberOfResources >= UnitHandler.instance.melee.baseStats.cost)
                        {
                            unitSpawningCount++;
                            barracks.GetComponent<EnemyBuilding>().SpawnUnit(UnitHandler.instance.melee);
                        }
                        break;
                    case 1:
                        if (numberOfResources >= UnitHandler.instance.ranged.baseStats.cost)
                        {
                            unitSpawningCount++;
                            barracks.GetComponent<EnemyBuilding>().SpawnUnit(UnitHandler.instance.ranged);
                        }
                        break;
                }
            }
        }
    }

    void CheckUnitNumber()
    {
        numberOfWorkers = enemyUnits.GetChild(0).childCount;
        numberOfUnits = 0;
        for (int i = 1; i <= 4; i++)
        {
            numberOfUnits += enemyUnits.GetChild(i).childCount;
        }
    }

    void AttackPlayer()
    {
        for (int i = 1; i < 3; i++)
        {
            foreach (Transform unit in enemyUnits.GetChild(i))
            {
                unit.GetComponent<EnemyUnit>().MoveUnit(approxEnemyBase);
                //set speed to match slowest member
            }
        }
    }

    bool CanAttackPlayer()
    {
        if (numberOfUnits < 10)
            return false;
        for (int i = 0; i < 2; i++)
        {
            foreach (Transform building in enemyBuildings.GetChild(i))
            {
                if (Physics.CheckSphere(building.position, 20, 8))
                    return false;
            }
        }
        return true;
    }

    public void RemoveResourceNode(GameObject node)
    {
        isRemovingResourceNode = true;
        resourceNodes.RemoveAt(0);
        GatherResources();
        Destroy(node);
    }
}
