using UnityEngine;
using UnityEngine.Events;

public class PickableNote : PickableBase
{
    [SerializeField] private ScriptableNote _noteData;
    public UnityEvent<ScriptableNote> OnNotePick;

    [SerializeField]
    public override void Interact()
    {
        base.Interact();
        OnNotePick?.Invoke(_noteData);
    }
}
