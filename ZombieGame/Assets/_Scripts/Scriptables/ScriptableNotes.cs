using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Note")]
public class ScriptableNote : ScriptableObject
{
    [SerializeField,TextArea] public string noteContent;





}
