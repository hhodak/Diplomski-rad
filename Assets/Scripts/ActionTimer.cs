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

    public IEnumerator SpawnQueueTimerPlayer(PlayerBuilding pb)
    {
        if (pb.spawnQueue.Count > 0)
        {
            yield return new WaitForSeconds(pb.spawnQueue[0]);

            pb.SpawnObject();

            if (pb.spawnQueue.Count > 0)
            {
                StartCoroutine(SpawnQueueTimerPlayer(pb));
            }
        }
    }

    public IEnumerator SpawnQueueTimerEnemy(EnemyBuilding eb)
    {
        if (eb.spawnQueue.Count > 0)
        {
            yield return new WaitForSeconds(eb.spawnQueue[0]);

            eb.SpawnObject();

            if (eb.spawnQueue.Count > 0)
            {
                StartCoroutine(SpawnQueueTimerEnemy(eb));
            }
        }
    }
}
