using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableNote : PickableBase
{
    [SerializeField] private ScriptableNote _noteData;

    public override void Interact()
    {
        base.Interact();
    }

}
