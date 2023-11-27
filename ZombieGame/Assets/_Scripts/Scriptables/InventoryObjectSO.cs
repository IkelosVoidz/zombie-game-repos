using UnityEngine;


public class InventoryObjectSO : ScriptableObject
{
    [Header("Inventory Properties")]
    [SerializeField] public string _name;
    [SerializeField] public string _type;
    [SerializeField, TextArea] public string _description;
}
