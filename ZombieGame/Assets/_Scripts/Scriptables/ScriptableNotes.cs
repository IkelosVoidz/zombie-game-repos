using UnityEngine;

[CreateAssetMenu(fileName = "Note")]
public class ScriptableNote : InventoryObjectSO
{
    [SerializeField, TextArea] public string noteContent;





}
