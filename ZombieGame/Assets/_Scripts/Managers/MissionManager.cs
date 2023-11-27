using System;
using System.Collections.Generic;
using UnityEngine;
public class MissionManager : StaticSingleton<MissionManager>
{
    private SortedList<string, SortedList<int, Mission>> _missions;
    [SerializeField] List<Mission> _missionsList;

    public void Awake()
    {
        foreach (Mission mis in _missionsList)
        {

        }
    }


    public void CompleteObjective(int id)
    {

    }

    //class objectve completer and thats what events call
}

[Serializable]
class Mission
{
    public string _name;
    public string _description;
    public List<MissonObjective> Objectives;
}

