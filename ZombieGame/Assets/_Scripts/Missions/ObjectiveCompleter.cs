using UnityEngine;

public class ObjectiveCompleter : MonoBehaviour
{
    [SerializeField] MissionObjective _missionToComplete;
    void CompleteObjective()
    {
        MissionManager.Instance.CompleteObjective(_missionToComplete);
    }
}
