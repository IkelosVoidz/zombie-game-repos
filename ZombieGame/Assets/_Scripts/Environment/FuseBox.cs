using UnityEngine;
using UnityEngine.Events;

public class FuseBox : MonoBehaviour, IInteractable
{
    [SerializeField] bool _isPoweredOn = false;

    [SerializeField] bool _outlineAffectChildren;

    private void Awake()
    {

    }

    public UnityEvent onFuseBoxUsedPoweredOff;
    public UnityEvent onFuseBoxUsedPoweredOn;
    public void Interact()
    {
        if (_isPoweredOn)
        {
            onFuseBoxUsedPoweredOn?.Invoke();
        }
        else onFuseBoxUsedPoweredOff?.Invoke();
    }

    public void ChangePoweredStatus()
    {
        _isPoweredOn = !_isPoweredOn;
        Debug.Log("_isPoweredOn: " + _isPoweredOn);
    }
}
