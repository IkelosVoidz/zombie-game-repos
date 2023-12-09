using UnityEngine;
using UnityEngine.Events;

public class PotionHandler : MonoBehaviour, IInteractable
{
    public UnityEvent onPotionMakerUsed;
    public UnityEvent onPotionMakerEnded;

    public void Interact()
    {
        onPotionMakerUsed?.Invoke();
    }

    public void OnTimerEnded()
    {
        onPotionMakerEnded?.Invoke();
    }

}
