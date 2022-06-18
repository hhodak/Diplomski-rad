using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IUnit : Interactable
{
    public PlayerActions playerActions;

    public override void OnInteractEnter()
    {
        ActionFrame.instance.SetActionButtons(playerActions, transform.gameObject);
        base.OnInteractEnter();
    }

    public override void OnInteractExit()
    {
        ActionFrame.instance.ClearButtons();
        base.OnInteractExit();
    }
}
