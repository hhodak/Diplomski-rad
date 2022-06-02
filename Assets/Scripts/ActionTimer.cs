using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTimer : MonoBehaviour
{
    public static ActionTimer instance = null;

    private void Awake()
    {
        instance = this;
    }

    public IEnumerator SpawnQueueTimer(PlayerBuilding pb)
    {
        if (pb.spawnQueue.Count > 0)
        {
            Debug.Log($"Waiting for {pb.spawnQueue[0]} seconds.");

            yield return new WaitForSeconds(pb.spawnQueue[0]);

            pb.SpawnObject();

            if (pb.spawnQueue.Count > 0)
            {
                StartCoroutine(SpawnQueueTimer(pb));
            }
        }
    }
}
