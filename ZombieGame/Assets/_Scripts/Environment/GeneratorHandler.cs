using UnityEngine;
using UnityEngine.Events;

public class GeneratorHandler : MonoBehaviour, IInteractable
{
    public UnityEvent onGeneratorUsed;
    public bool oneTime;
    bool active;

    public void Interact()
    {
        if (oneTime && active) return;

        onGeneratorUsed?.Invoke();
        active = !active;
    }
}
