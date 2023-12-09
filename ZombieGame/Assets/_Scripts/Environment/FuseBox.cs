using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FuseBox : MonoBehaviour, IInteractable
{
    [SerializeField] bool _isPoweredOn = false;

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
