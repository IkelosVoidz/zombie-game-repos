using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PickableNote : PickableBase
{
    [SerializeField] private ScriptableNote _noteData;
    public UnityEvent<ScriptableNote> OnNotePick;
    public override void Interact()
    {
        base.Interact();
        OnNotePick?.Invoke(_noteData);
    }

}
