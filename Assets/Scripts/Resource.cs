using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public float amount;
    private float resourcePerScale = 250;

    private void Start()
    {
        ScaleResourceObject();
    }

    public float GatheredResources(float gatheringAmount)
    {
        float gatheredAmount = gatheringAmount > amount ? amount : gatheringAmount;
        amount -= gatheringAmount > amount ? amount : gatheringAmount;
        ScaleResourceObject();
        return gatheredAmount;
    }

    void ScaleResourceObject()
    {
        float scale = Mathf.Floor(amount / resourcePerScale) + 1;
        transform.localScale = new Vector3(scale, 0.1f, scale);
    }
}
