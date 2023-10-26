using JetBrains.Annotations;
using System;
using UnityEngine;

public class GeneratorHandler : MonoBehaviour, IInteractable
{
    public DoorController controller;
    public void Interact()
    {
        controller.ChangeBlockedStatus();
        /*controller.Interact();
        controller.ChangeBlockedStatus();*/
    }

}
