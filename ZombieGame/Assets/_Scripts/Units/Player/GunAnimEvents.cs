using System;
using UnityEngine;

public class GunAnimEvents : MonoBehaviour
{
    public static event Action OnReloadAnimEnd;
    public static event Action OnMeleeAnimHit;
    public static event Action OnMeleeAnimEnd;
    public void ReloadAnimEndEvent()
    {
        OnReloadAnimEnd?.Invoke();
    }

    public void OnMeleeHit()
    {
        OnMeleeAnimHit?.Invoke();
    }

    public void OnMeleeEnd()
    {
        OnMeleeAnimEnd?.Invoke();
    }
}
