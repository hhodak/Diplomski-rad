using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitStatDisplay : MonoBehaviour
{
    public float health, armor, currentHealth;
    [SerializeField] private Image healthBar;
    bool isPlayerUnit = false;

    public void SetStatDisplayBasicUnit(UnitStatTypes.Base stats, bool isPlayer)
    {
        health = stats.health;
        armor = stats.armor;
        isPlayerUnit = isPlayer;
        currentHealth = health;

        ResourceManager.instance.AddXP(stats.cost);
    }

    public void SetStatDisplayBasicBuilding(BuildingStatTypes.Base stats, bool isPlayer)
    {
        health = stats.health;
        armor = stats.armor;
        isPlayerUnit = isPlayer;
        currentHealth = health;
    }

    void Update()
    {
        HandleHealth();
    }

    public void TakeDamage(float damage)
    {
        float totalDamage = damage - armor;
        if (totalDamage > 0)
        {
            currentHealth -= totalDamage;
        }

    }

    void HandleHealth()
    {
        Camera camera = Camera.main; //fix
        gameObject.transform.LookAt(gameObject.transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);
        healthBar.fillAmount = currentHealth / health;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isPlayerUnit)
        {
            InputHandler.instance.selectedUnits.Remove(gameObject.transform.parent);
            InputHandler.instance.RemoveDestroyedUnitFromHotkey(gameObject.transform.parent);
            Destroy(gameObject.transform.parent.gameObject);
        }
        else
        {
            ResourceManager.instance.AddXP(gameObject.transform.parent.GetComponent<EnemyUnit>().baseStats.cost);
            Destroy(gameObject.transform.parent.gameObject);
        }
    }
}
