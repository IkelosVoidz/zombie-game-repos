using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

[Serializable]
[CreateAssetMenu(fileName = "MissionObjecive", menuName = "MissionObjective", order = 0)]
public class MissionObjective : ScriptableObject
{
    public string _name;
    [TextArea] public string _description;
    public string _mission;
    [HideInInspector] public int _order;
    public bool _completed;

    private void Awake()
    {
        _completed = false;
        Debug.Log(name + " _completed: " + _completed);
    }

    public virtual void OnSelected()
    {
        //play radio message or whatever
    }

    public virtual void OnCompleted()
    {
        //play radio message or whatever
    }

    public virtual void OnUpdate() //que se llame o del mismo mission manager o usando eventos de c# en los HIJOS
    {

    }
}