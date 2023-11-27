using System;
using UnityEngine;

[Serializable]
public class MissonObjective : ScriptableObject
{
    public string _name;
    public string _description;
    public int _order;
    public bool _completed;

    public virtual void OnSelected()
    {
        //play radio message or whatever
    }

    public virtual void OnCompleted()
    {
        //play radio message or whatever
    }

    public virtual void OnUpdate() //de hecho creo que esto es mejor que se haga con eventos de c# a los que se suscriban los HIJOS solo
    {

    }
}