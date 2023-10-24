using UnityEngine;

public class PickableBase : MonoBehaviour, IInteractable
{

    /// <summary>
    /// Deletes the objected selected
    /// </summary>
    public virtual void Interact()
    {
        gameObject.HideAndDestroyAfterDelay(1);
    }
}
