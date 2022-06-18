using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Projectile : MonoBehaviour
{
    public enum ProjectileType
    {
        Arrow,
        CannonBall
    }

    float movementSpeed = 10;
    public Vector3 destination;
    public ProjectileType type;
    private List<Transform> affectedUnits = new List<Transform>();
    private List<Transform> affectedBuildings = new List<Transform>();
    private float damageRadius;

    private void Start()
    {
        damageRadius = GetComponent<SphereCollider>().radius;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Launch(destination, type, 50);
        }
    }

    public void Launch(Vector3 targetPosition, ProjectileType type, float damageAmount)
    {
        float distance = Vector3.Distance(targetPosition, transform.position);
        float duration = distance / movementSpeed;
        float jumpHeight = 1 + (0.1f * distance);
        transform.DOJump(targetPosition, jumpHeight, 1, duration);
        switch (type)
        {
            case ProjectileType.Arrow:
                StartCoroutine(DestroyProjectile(duration));
                break;
            case ProjectileType.CannonBall:
                StartCoroutine(Explode(duration, damageAmount));
                break;
        }
    }

    IEnumerator DestroyProjectile(float sec)
    {
        yield return new WaitForSeconds(sec);
        Destroy(gameObject);
    }

    IEnumerator Explode(float sec, float damageAmount)
    {
        yield return new WaitForSeconds(sec);
        ShowDustCloud();
        DealDamage(damageAmount);
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

    void DealDamage(float damageAmount)
    {
        foreach (var unit in affectedUnits)
        {
            float distance = Vector3.Distance(transform.position, unit.position);
            float damagePercentage = (float)System.Math.Round(Mathf.Clamp01(1 - (distance / damageRadius)), 2);
            UnitStatDisplay usd = unit.gameObject.GetComponentInChildren<UnitStatDisplay>();
            float totalDamage = damagePercentage * damageAmount;
            usd.TakeDamage(totalDamage);
        }
        foreach (var building in affectedBuildings)
        {
            float distance = Vector3.Distance(transform.position, building.position);
            float damagePercentage = (float)System.Math.Round(Mathf.Clamp01(1 - (distance / damageRadius)), 2);
            UnitStatDisplay usd = building.gameObject.GetComponentInChildren<UnitStatDisplay>(); //check if it is BuildingStatDisplay
            float totalDamage = damagePercentage * damageAmount * 1.25f;
            usd.TakeDamage(totalDamage);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Unit") && !affectedUnits.Contains(other.transform))
        {
            affectedUnits.Add(other.transform);
        }
        if (other.gameObject.tag.Equals("Building") && !affectedBuildings.Contains(other.transform))
        {
            affectedBuildings.Add(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Unit"))
        {
            affectedUnits.Remove(other.transform);
        }
        if (other.gameObject.tag.Equals("Building"))
        {
            affectedBuildings.Remove(other.transform);
        }
    }

    void ShowDustCloud()
    {
        GetComponent<MeshRenderer>().enabled = false;
        Transform dust = transform.GetChild(0);
        dust.gameObject.SetActive(true);
    }
}
