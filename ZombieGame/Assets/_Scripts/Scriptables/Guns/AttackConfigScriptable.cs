using UnityEngine;

[CreateAssetMenu(fileName = "Base Attack Config", menuName = "Weapons/Attack Configuration/Base", order = 0)]
public class AttackConfigScriptable : ScriptableObject
{
    public AttackType _attackType;
    public LayerMask _hitMask;
    public Vector3 _spread = new Vector3(0.1f, 0.1f, 0.1f);
    public float _fireRate = 0.25f;
    public bool _fullAuto;

    [HideInInspector]
    public float _lastAttackTime;

    private Transform _lookOrientation;
    private ParticleSystem _gunTip;

    public virtual void Attack(bool inputHeld)
    {
        if (!_fullAuto && inputHeld) return;

        if (Time.time > _fireRate + _lastAttackTime)
        {
            _lastAttackTime = Time.time;
            _gunTip.Play();

            Vector3 spreadAmount = new Vector3(
                Random.Range(-_spread.x, _spread.x),
                Random.Range(-_spread.y, _spread.y),
                Random.Range(-_spread.z, _spread.z)
                );

        }
    }

    public void InitializeAttackConfig(Transform lookOrientation, ParticleSystem gunTip)
    {
        _lookOrientation = lookOrientation;
        _gunTip = gunTip;
    }

    public Vector3 GetRayCastOrigin()
    {
        Vector3 origin = _lookOrientation.position + _lookOrientation.forward *
            Vector3.Distance(_lookOrientation.position, _gunTip.transform.position);

        return origin;
    }

    protected virtual bool ShootBullet(Vector3 spreadAmount, out RaycastHit hitInfo)
    {
        Vector3 shootDirection = _lookOrientation.transform.forward + _lookOrientation.TransformDirection(spreadAmount);

        shootDirection.Normalize();

        if (Physics.Raycast( //el pew pew
            GetRayCastOrigin(),
            shootDirection,
             out RaycastHit hit,
             float.MaxValue,
             _hitMask
            ))
        {
        }
        hitInfo = hit;
        return true;
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

