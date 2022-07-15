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

    [SerializeField]
    bool isPlayer;
    public Transform resourceNode;
    public Transform commandCenter;
    public float cargo = 0;
    public int maxCargoAmount = 5;
    private PlayerUnit playerUnit;
    private EnemyUnit enemyUnit;
    public WorkerState state = WorkerState.Other;
    public GameObject cargoGO;
    public GameObject dustParticles;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (isPlayer)
        {
            playerUnit = GetComponent<PlayerUnit>();
            playerUnit.StopUnit();
        }
        else
        {
            enemyUnit = GetComponent<EnemyUnit>();
        }
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
            GameObject parent = isPlayer ? GameObject.Find("PlayerCommandCenter") : GameObject.Find("EnemyCommandCenter");
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
                if (isPlayer)
                    playerUnit.MoveUnit(resourceNode.position);
                else
                    enemyUnit.MoveUnit(resourceNode.position);
                break;
            case WorkerState.ReturningCargo:
                if (isPlayer)
                    playerUnit.MoveUnit(commandCenter.position);
                else
                    enemyUnit.MoveUnit(commandCenter.position);
                break;
            case WorkerState.Other:
                if (isPlayer)
                    playerUnit.StopUnit();
                else
                    enemyUnit.StopUnit();
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
        if (isPlayer)
            playerUnit.StopUnit();
        else
            enemyUnit.StopUnit();
        dustParticles.SetActive(true);
        PlaySound();
        yield return new WaitForSeconds(2);
        StopPlayingSound();
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
            if (!isPlayer && !EnemyManager.isRemovingResourceNode)
            {
                EnemyManager.isRemovingResourceNode = true;
                EnemyManager.instance.RemoveResourceNode(resourceNode.gameObject);
            }
            if (isPlayer)
            {
                if (resourceNode)
                {
                    Destroy(resourceNode.gameObject);
                    resourceNode = null;
                }
            }
        }
    }

    IEnumerator ReturnCargo()
    {
        if (isPlayer)
            playerUnit.StopUnit();
        else
            enemyUnit.StopUnit();
        cargoGO.SetActive(false);
        yield return new WaitForSeconds(1);
        if (isPlayer)
            ResourceManager.instance.AddResources(cargo);
        else
            EnemyManager.instance.numberOfResources += cargo;
        cargo = 0;
    }

    public void SpawnBuilding(BasicBuilding building)
    {
        if (building.baseStats.cost <= ResourceManager.instance.currentResources)
        {
            SpawnBlueprint(building.buildingBlueprintPrefab);
        }
        else
        {
            LogController.instance.ShowMessage("Not enough resources!");
        }
    }

    void SpawnBlueprint(GameObject blueprint)
    {
        Instantiate(blueprint);
    }

    void PlaySound()
    {
        audioSource.Play();
    }

    void StopPlayingSound()
    {
        audioSource.Stop();
    }
}
