using UnityEngine;
using UnityEngine.Events;

public class PotionHandler : MonoBehaviour, IInteractable
{
    public UnityEvent onPotionMakerUsed;

    public void Interact()
    {
        onPotionMakerUsed?.Invoke();
    }
}
