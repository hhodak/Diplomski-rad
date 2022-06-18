using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitStatDisplay : MonoBehaviour
{
    public float health, armor, currentHealth, energy, currentEnergy;
    [SerializeField] private Image healthBar;
    [SerializeField] private Image energyBar;
    bool isPlayerUnit = false;
    static bool hasPassivelyRestored = true;

    public void SetStatDisplayBasicUnit(UnitStatTypes.Base stats, bool isPlayer)
    {
        health = stats.health;
        energy = stats.energy;
        armor = stats.armor;
        isPlayerUnit = isPlayer;
        currentHealth = health;
        currentEnergy = energy;

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
        HandleHealthAndEnergy();
        if (hasPassivelyRestored)
        {
            hasPassivelyRestored = false;
            StartCoroutine(PassiveEnergyRestoration());
        }
    }

    public void TakeDamage(float damage)
    {
        float totalDamage = damage - armor;
        if (totalDamage > 0)
        {
            currentHealth -= totalDamage;
        }
    }

    public void RestoreHealth(float healthAmount)
    {
        currentHealth += healthAmount;
        if (currentHealth > health)
        {
            currentHealth = health;
        }
    }

    void HandleHealthAndEnergy()
    {
        Camera camera = Camera.main; //fix
        gameObject.transform.LookAt(gameObject.transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);
        healthBar.fillAmount = currentHealth / health;
        energyBar.fillAmount = currentEnergy / energy;
        ChangeColor();
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void ChangeColor()
    {
        if (currentHealth < health / 3)
        {
            healthBar.color = Color.red;
        }
        else if (currentHealth < 2 * health / 3)
        {
            healthBar.color = Color.yellow;
        }
        else
        {
            healthBar.color = Color.green;
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

    public void ReduceEnergy(float amount)
    {
        currentEnergy -= amount;
    }

    public void RestoreEnergy(float amount)
    {
        currentEnergy += amount;
        if (currentEnergy > energy)
            currentEnergy = energy;
    }

    IEnumerator PassiveEnergyRestoration()
    {
        yield return new WaitForSeconds(3); //new property?
        RestoreEnergy(1);
        hasPassivelyRestored = true;
    }
}
