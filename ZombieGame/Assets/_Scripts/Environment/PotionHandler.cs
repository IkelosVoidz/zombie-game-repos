using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PotionHandler : MonoBehaviour, IInteractable
{
    public UnityEvent onGeneratorUsed;

    public void Interact()
    {
        onGeneratorUsed?.Invoke();
    }
}
