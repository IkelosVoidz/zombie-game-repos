using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.Events;

public class GeneratorHandler : MonoBehaviour, IInteractable
{
    public UnityEvent onGeneratorUsed;

    public void Interact()
    {
        onGeneratorUsed?.Invoke();
    }

}
