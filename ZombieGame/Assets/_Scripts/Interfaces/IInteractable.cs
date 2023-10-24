using Unity.VisualScripting;
using UnityEngine;
public interface IInteractable
{
    /// <summary>
    /// Reference to the game object on which this Interface is implemented on
    /// </summary>
    GameObject gameObject { get; }
    void Interact();

    void OnSelect()
    {
        if (!gameObject.IsDestroyed())
        {
            gameObject.SwapLayer("Outline", true);
        }
    }

    void OnDeselect()
    {
        if (!gameObject.IsDestroyed())
        {
            gameObject.SwapLayer("Interactable", true);
        }
    }
}
