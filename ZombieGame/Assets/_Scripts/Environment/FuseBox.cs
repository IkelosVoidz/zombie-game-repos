using UnityEngine;
using UnityEngine.Events;

public class FuseBox : MonoBehaviour, IInteractable
{
    [SerializeField] bool _isPoweredOn = false;
    [SerializeField] AudioClip clickFuseBox;

    public UnityEvent onFuseBoxUsedPoweredOff;
    public UnityEvent onFuseBoxUsedPoweredOn;
    public void Interact()
    {
        SoundManager.Instance.PlaySoundFXClip(clickFuseBox, transform, 1f);

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
