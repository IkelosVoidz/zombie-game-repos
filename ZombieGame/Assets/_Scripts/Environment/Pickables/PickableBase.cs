using System;
using UnityEngine;

public class PickableBase : MonoBehaviour, IInteractable
{

    /// <summary>
    /// Deletes the objected selected
    /// </summary>
    /// 

    public static event Action<SpriteRenderer> OnPickablePicked;
    [SerializeField] private AudioClip _pickupSound;

    public virtual void Interact()
    {
        SoundManager.Instance.PlaySoundFXClip(_pickupSound, transform, 1.0f);
        gameObject.GetComponentInChildren<MinimapIconStats>()._disabled = true;
        OnPickablePicked?.Invoke(gameObject.GetComponentInChildren<SpriteRenderer>());
        gameObject.HideAndDestroyAfterDelay(1);
    }
}
