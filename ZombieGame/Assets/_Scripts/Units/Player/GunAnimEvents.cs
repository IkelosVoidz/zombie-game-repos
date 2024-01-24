using System;
using UnityEngine;

public class GunAnimEvents : MonoBehaviour
{
    public static event Action OnReloadAnimEnd;
    public static event Action OnMeleeAnimHit;
    public static event Action OnMeleeAnimEnd;
    public static event Action OnSwapAnimEnd;

    private Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }


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

    public void OnSwapEnd()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).speed == 1)
        {
            OnSwapAnimEnd?.Invoke();
        }
        else
        {
            //Debug.Log("hola");
        }
    }
}
