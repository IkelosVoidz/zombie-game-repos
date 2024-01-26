using System;
using UnityEngine;

public class PickableBase : MonoBehaviour, IInteractable
{

    /// <summary>
    /// Deletes the objected selected
    /// </summary>
    /// 

    public static event Action<SpriteRenderer> OnPickablePicked;

    public virtual void Interact()
    {
        gameObject.GetComponentInChildren<MinimapIconStats>()._disabled = true;
        OnPickablePicked?.Invoke(gameObject.GetComponentInChildren<SpriteRenderer>());
        gameObject.HideAndDestroyAfterDelay(1);
    }
}
