using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableBase : MonoBehaviour,IInteractable
{
    
    /// <summary>
    /// Deletes the objected selected
    /// </summary>
    public virtual void Interact()
    {
        Destroy(gameObject);

    }
}
