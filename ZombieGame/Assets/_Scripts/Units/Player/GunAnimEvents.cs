using System;
using UnityEngine;

public class GunAnimEvents : MonoBehaviour
{
    public static event Action OnReloadAnimEnd;
    public void ReloadAnimEndEvent()
    {
        OnReloadAnimEnd?.Invoke();
    }
}
