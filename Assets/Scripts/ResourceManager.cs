using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager instance;

    public float maxXP;
    public float currentXP;
    public float currentResources;

    public Slider xpSlider;
    public Text xpText;
    public Text resourcesText;

    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        xpSlider.value = currentXP / maxXP;
        xpText.text = currentXP.ToString();
        resourcesText.text = currentResources.ToString();
    }

    public void AddXP(float xpAmount)
    {
        currentXP += xpAmount;
        if (currentXP >= maxXP)
        {
            xpSlider.value = 1;
        }
        else
        {
            xpSlider.value = currentXP / maxXP;
        }
        xpText.text = currentXP.ToString("F2");
    }

    public void AddResources(float resourcesAmount)
    {
        currentResources += resourcesAmount;
        GameManager.GameStats.resourcesMined += resourcesAmount;
        resourcesText.text = currentResources.ToString("F2");
    }

    public void SubtractResource(float resourcesAmount)
    {
        currentResources -= (resourcesAmount < currentResources) ? resourcesAmount : currentResources;
        GameManager.GameStats.resourcesSpent += resourcesAmount;
        resourcesText.text = currentResources.ToString("F2");
    }
}
