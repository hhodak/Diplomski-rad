using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    [HideInInspector]
    public List<Transform> transforms = new List<Transform>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Unit") && !transforms.Contains(other.transform))
        {
            transforms.Add(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Unit"))
        {
            transforms.Remove(other.transform);
        }
    }
}
