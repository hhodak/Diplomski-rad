using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : MonoBehaviour
{
    public enum WorkerState
    {
        Other,
        GoingToResource,
        Gathering,
        ReturningCargo
    }

    public Transform resourceNode;
    public Transform commandCenter;
    public float cargo = 0;
    public int maxCargoAmount = 5;
    private PlayerUnit playerUnit;
    public WorkerState state = WorkerState.Other;
    public GameObject cargoGO;
    public GameObject dustParticles;

    private void Start()
    {
        playerUnit = GetComponent<PlayerUnit>();
        playerUnit.StopUnit();
    }

    private void Update()
    {
        if (!resourceNode && (state == WorkerState.GoingToResource || state == WorkerState.Gathering))
        {
            ChangeWorkerState(WorkerState.Other);
        }
    }

    public void HandleWorking(bool isWorkOrdered, Transform resource = null)
    {
        if (isWorkOrdered)
        {
            resourceNode = resource;
            GameObject parent = GameObject.Find("PlayerCommandCenter");
            commandCenter = parent.transform.GetChild(0);
        }
        else
        {
            resourceNode = null;
            commandCenter = null;
        }

        ChangeWorkerState(isWorkOrdered ? WorkerState.GoingToResource : WorkerState.Other);
    }

    void ChangeWorkerState(WorkerState workerState)
    {
        state = workerState;
        switch (state)
        {
            case WorkerState.Gathering:
                StartCoroutine(GatherResources());
                break;
            case WorkerState.GoingToResource:
                playerUnit.MoveUnit(resourceNode.position);
                break;
            case WorkerState.ReturningCargo:
                playerUnit.MoveUnit(commandCenter.position);
                break;
            case WorkerState.Other:
                playerUnit.StopUnit();
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("CommandCenter") && state == WorkerState.ReturningCargo)
        {
            StartCoroutine(ReturnCargo());
            if (resourceNode)
            {
                ChangeWorkerState(WorkerState.GoingToResource);
            }
            else
            {
                ChangeWorkerState(WorkerState.Other);
            }
            return;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Resource") && state == WorkerState.GoingToResource)
        {
            ChangeWorkerState(WorkerState.Gathering);
            return;
        }
    }

    IEnumerator GatherResources()
    {
        playerUnit.StopUnit();
        dustParticles.SetActive(true);
        yield return new WaitForSeconds(2);
        cargo = resourceNode.GetComponent<Resource>().GatheredResources(maxCargoAmount);
        CheckRemainingResouces();
        ChangeWorkerState(WorkerState.ReturningCargo);
        cargoGO.SetActive(true);
        dustParticles.SetActive(false);
    }

    void CheckRemainingResouces()
    {
        if (resourceNode.GetComponent<Resource>().amount == 0)
        {
            Destroy(resourceNode.gameObject);
            resourceNode = null;
        }
    }

    IEnumerator ReturnCargo()
    {
        playerUnit.StopUnit();
        yield return new WaitForSeconds(1);
        ResourceManager.instance.AddResources(cargo);
        cargo = 0;
        cargoGO.SetActive(false);
    }

    public void SpawnBuilding(BasicBuilding building)
    {
        if (building.baseStats.cost <= ResourceManager.instance.currentResources)
        {
            SpawnBlueprint(building.buildingBlueprintPrefab);
            StartCoroutine(LogController.instance.ShowMessage($"{building.buildingName} added to the construction queue."));

            ResourceManager.instance.SubtractResource(building.baseStats.cost);
        }
        else
        {
            StartCoroutine(LogController.instance.ShowMessage("Not enough resources!"));
        }
    }

    void SpawnBlueprint(GameObject blueprint)
    {
        Instantiate(blueprint);
    }
}
