using UnityEngine;
using UnityEngine.Events;

public class TriggerDoSomething : MonoBehaviour
{
    public UnityEvent onTriggerDoSomething;

    private void OnTriggerEnter(Collider other)
    {
        onTriggerDoSomething?.Invoke();
    }
}
