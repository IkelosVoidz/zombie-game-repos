using System;
using UnityEngine;

public class GunAnimEvents : MonoBehaviour
{
    public static event Action OnReloadAnimEnd;
    public static event Action OnMeleeAnimHit;
    public static event Action OnMeleeAnimEnd;
    public static event Action OnSwapAnimEnd;


    public static event Action OnMagDropAnim;
    public static event Action OnMagInsertAnim;
    public static event Action OnChamberAnim;

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


    public void MagDropAnimEvent()
    {
        OnMagDropAnim?.Invoke();
    }

    public void MagInsertAnimEvent()
    {
        OnMagInsertAnim?.Invoke();
    }

    public void ChamberAnimEvent()
    {
        OnChamberAnim?.Invoke();
    }
}
