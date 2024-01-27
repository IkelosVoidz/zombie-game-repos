using UnityEngine;
using UnityEngine.Events;

public class GeneratorHandler : MonoBehaviour, IInteractable
{
    [SerializeField] AudioClip startEngine;

    public UnityEvent onGeneratorUsed;
    public bool oneTime;
    bool active;

    public void Interact()
    {
        if (oneTime && active) return;
        if (!active) SoundManager.Instance.PlaySoundFXClip(startEngine, transform, .7f);

        onGeneratorUsed?.Invoke();
        active = !active;

    }
}
