using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool isInteracting = false;
    public GameObject highlight = null;

    public virtual void Awake()
    {
        highlight.SetActive(false);
    }

    public virtual void OnInteractEnter()
    {
        ShowHighlight(true);
        isInteracting = true;
    }

    public virtual void OnInteractExit()
    {
        ShowHighlight(false);
        isInteracting = false;
    }

    public virtual void ShowHighlight(bool isOn)
    {
        highlight.SetActive(isOn);
    }
}
