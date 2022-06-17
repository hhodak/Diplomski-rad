using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Siege : MonoBehaviour
{
    public GameObject projectilePrefab;

    public void SiegeAttack(Transform target, float damage)
    {
        if (IsInSiegeDistance(target.position))
        {
            GameObject newProjectile = Instantiate(projectilePrefab, transform.position, transform.rotation, transform);
            Projectile projectile = newProjectile.GetComponent<Projectile>();
            projectile.Launch(target.position, Projectile.ProjectileType.CannonBall, damage);
        }
    }

    bool IsInSiegeDistance(Vector3 targetPosition)
    {
        float minAttackRange = GetComponent<PlayerUnit>().siegeStats.minimumAttackRange;
        float distance = Vector3.Distance(targetPosition, transform.position);
        if (minAttackRange > distance)
        {
            Debug.Log("Too close for siege attack.");
            return false;
        }
        return true;
    }
}
