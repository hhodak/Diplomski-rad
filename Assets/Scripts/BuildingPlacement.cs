using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacement : MonoBehaviour
{
    public List<Collider> colliders = new List<Collider>();
    public List<string> blockingTags = new List<string>(){
        "Unit",
        "Building",
        "Resource",
        "Environment"
        };

    private void OnTriggerEnter(Collider other)
    {
        if (blockingTags.Contains(other.gameObject.tag))
        {
            colliders.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (blockingTags.Contains(other.gameObject.tag))
        {
            colliders.Remove(other);
        }
    }
}
