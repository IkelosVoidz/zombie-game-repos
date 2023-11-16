using System;
using UnityEngine;

public class AttackConfigScriptable : ScriptableObject
{
    public AttackType _attackType;
    public float _fireRate = 0.25f;
    public bool _fullAuto;

    [HideInInspector]
    public float _lastAttackTime;

    protected Transform _lookOrientation;
    //same with this 
    protected ParticleSystem _gunTip;
    protected DamageConfigScriptable _damageConfig;
    //will need to figure out how to make this work when its null 
    protected ReloadConfigScriptable _reloadConfig;


    //EVENTS
    /// <summary>
    /// When the active weapon shoots
    /// </summary>
    public static event Action<AmmoData> OnShoot;

    public virtual void Attack(bool inputHeld)
    {
        if (!_fullAuto && inputHeld) return;
        if (!CanAttack()) return;

        _lastAttackTime = Time.time;
        _gunTip.Play();
        OnShoot?.Invoke(_reloadConfig._ammo);
    }


    protected virtual bool CanAttack()
    {
        return (Time.time > _fireRate + _lastAttackTime);
    }

    public void InitializeAttackConfig(Transform lookOrientation, ParticleSystem gunTip, DamageConfigScriptable dmgCfg, ReloadConfigScriptable reloadCfg)
    {
        _lookOrientation = lookOrientation;
        _gunTip = gunTip;
        _damageConfig = dmgCfg;
        _reloadConfig = reloadCfg;
    }

    protected virtual void ApplyDamage(HealthComponent health, float distance, Vector3 attackDir)
    {
        health.TakeDamage(_damageConfig.GetDamage(distance), attackDir);
    }
}


public enum AttackType //will change 
{
    SemiAuto,
    FullAuto,
    Burst,
    Projectile,
    Melee
}

