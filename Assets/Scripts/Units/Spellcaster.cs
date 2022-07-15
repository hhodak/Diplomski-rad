using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spellcaster : MonoBehaviour
{
    public GameObject healingPrefab;
    GameObject heal;
    Ability ability;
    Transform healingTagret = null;
    PlayerUnit playerUnit;
    bool hasHealingTarget = false;

    private void Start()
    {
        playerUnit = GetComponent<PlayerUnit>();
    }

    private void Update()
    {
        if (healingTagret)
        {
            float dist = Vector3.Distance(transform.position, healingTagret.position);
            if (ability.baseStats.range > dist)
            {
                Heal();
            }
            else
            {
                playerUnit.MoveUnit(healingTagret.position);
            }
        }
        if (!healingTagret && hasHealingTarget)
        {
            hasHealingTarget = false;
            playerUnit.StopUnit();
            StartCoroutine(DestroyHeal());
        }
    }

    public void HealUnit(Transform target)
    {
        healingTagret = target;
        hasHealingTarget = true;
        heal = Instantiate(healingPrefab, target.position, Quaternion.identity, transform);
        ability = heal.GetComponent<Ability>();
    }

    void Heal()
    {
        UnitStatDisplay usd = transform.gameObject.GetComponentInChildren<UnitStatDisplay>();

        if (usd.currentEnergy >= ability.abilityType.baseStats.cost)
        {
            usd.ReduceEnergy(ability.abilityType.baseStats.cost);
            heal.transform.position = healingTagret.position;
            ability.HealTarget(healingTagret);
            StartCoroutine(DestroyHeal());
        }
        else
        {
            LogController.instance.ShowMessage("Not enough energy!");
            Destroy(heal);
        }
        healingTagret = null;
        hasHealingTarget = false;
        playerUnit.StopUnit();
    }

    IEnumerator DestroyHeal()
    {
        yield return new WaitForSeconds(ability.baseStats.duration);
        Destroy(heal);
        heal = null;
        ability = null;
    }
}
