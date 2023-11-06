using UnityEngine;
using UnityEngine.Events;

public class PickableNote : PickableBase
{
    [SerializeField] private ScriptableNote _noteData;
    public UnityEvent<ScriptableNote> OnNotePick;

    [SerializeField]
    private HandleActionMaps referencia;
    public override void Interact()
    {
        base.Interact();
        //referencia.SwitchActionMap("UI");
        OnNotePick?.Invoke(_noteData);
    }
}
