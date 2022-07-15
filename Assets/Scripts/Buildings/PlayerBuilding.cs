using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//[RequireComponent(typeof(NavMeshAgent))]
public class PlayerBuilding : MonoBehaviour
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
        statDisplay.SetStatDisplayBasicBuilding(baseStats, true);
    }

    public void SpawnUnit(BasicUnit unit)
    {
        if (unit.baseStats.cost <= ResourceManager.instance.currentResources)
        {
            spawnQueue.Add(unit.spawnTime);
            spawnOrder.Add(unit.cubePrefab);
            LogController.instance.ShowMessage($"{unit.unitName} added to the building queue.");
            ResourceManager.instance.SubtractResource(unit.baseStats.cost);
        }
        else
        {
            LogController.instance.ShowMessage("Not enough resources!");
        }

        if (spawnQueue.Count == 1)
        {
            ActionTimer.instance.StartCoroutine(ActionTimer.instance.SpawnQueueTimerPlayer(this));
        }
        else if (spawnQueue.Count == 0)
        {
            ActionTimer.instance.StopAllCoroutines();
        }
    }

    public void SpawnObject()
    {
        GameObject spawnedObject = Instantiate(spawnOrder[0], spawnPoint.transform.parent.position + buildingType.spawnLocation, Quaternion.identity);
        GameManager.GameStats.unitsBuilt++;

        PlayerUnit playerUnit = spawnedObject.GetComponent<PlayerUnit>();
        UnitStatDisplay usd = spawnedObject.gameObject.GetComponentInChildren<UnitStatDisplay>();
        usd.PlaySoundUnit("Spawn");
        playerUnit.transform.SetParent(GameObject.Find("Player" + playerUnit.unitType.unitType.ToString()).transform);

        playerUnit.MoveUnit(spawnPoint.transform.position);
        spawnQueue.Remove(spawnQueue[0]);
        spawnOrder.Remove(spawnOrder[0]);
    }
}
