using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBuilding : MonoBehaviour
{
    public BasicBuilding buildingType;

    [HideInInspector]
    public BuildingStatTypes.Base baseStats;
    public UnitStatDisplay statDisplay;

    public List<float> spawnQueue = new List<float>();
    public List<GameObject> spawnOrder = new List<GameObject>();
    public GameObject spawnPoint = null;

    private void Start()
    {
        baseStats = buildingType.baseStats;
        spawnPoint = transform.GetChild(2).gameObject;
        statDisplay.SetStatDisplayBasicBuilding(baseStats, false);
    }

    public void SpawnUnit(BasicUnit unit)
    {
        if (unit.baseStats.cost <= EnemyManager.instance.numberOfResources)
        {
            spawnQueue.Add(unit.spawnTime);
            spawnOrder.Add(unit.spherePrefab);
            EnemyManager.instance.numberOfResources -= unit.baseStats.cost;
        }

        if (spawnQueue.Count == 1)
        {
            ActionTimer.instance.StartCoroutine(ActionTimer.instance.SpawnQueueTimerEnemy(this));
        }
        else if (spawnQueue.Count == 0)
        {
            ActionTimer.instance.StopAllCoroutines();
        }
    }

    public void SpawnObject()
    {
        GameObject spawnedObject = Instantiate(spawnOrder[0], spawnPoint.transform.parent.position + buildingType.spawnLocation, Quaternion.identity);

        EnemyUnit enemyUnit = spawnedObject.GetComponent<EnemyUnit>();
        enemyUnit.transform.SetParent(GameObject.Find("Enemy" + enemyUnit.unitType.unitType.ToString()).transform);

        enemyUnit.MoveUnit(spawnPoint.transform.position);
        spawnQueue.Remove(spawnQueue[0]);
        spawnOrder.Remove(spawnOrder[0]);
    }
}
