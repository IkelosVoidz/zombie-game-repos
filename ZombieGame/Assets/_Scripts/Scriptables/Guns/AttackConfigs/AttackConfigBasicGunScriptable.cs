using UnityEngine;

[CreateAssetMenu(fileName = "Basic Gun Attack Config", menuName = "Weapons/Attack Configuration/Basic Gun", order = 0)]
public class AttackConfigBasicGunScriptable : AttackConfigScriptable
{
    [Header("Basic Hitscan Gun properties")]

    [SerializeField]
    protected LayerMask _hitMask;
    [SerializeField]
    protected Vector3 _spread = new Vector3(0.1f, 0.1f, 0.1f);


    public override void Attack(bool inputHeld)
    {
        base.Attack(inputHeld);
        //esta lina solo se ejecutara pasado el cooldown
        ShootHitscanBullet();
    }

    public Vector3 GetRayCastOrigin() //habra que hacer que se pueda setear que salga del arma por si queremos hacer eso en algun momento
    {
        Vector3 origin = _lookOrientation.position;


        return origin;
    }

    /*public Vector3 GetRaycastDirection()
    {

    }*/

    protected virtual void ShootHitscanBullet()
    {
        Vector3 shootDirection = _lookOrientation.transform.forward + _lookOrientation.TransformDirection(CalculateSpread());

        shootDirection.Normalize();

        if (Physics.Raycast( //el pew pew
            GetRayCastOrigin(),
            shootDirection,
             out RaycastHit hit,
             float.MaxValue,
             _hitMask
            ))
        {
            HandleHitEffects(hit, shootDirection);
            //also SurfaceManager handle impact
        }
    }

    protected virtual void HandleHitEffects(RaycastHit hitInfo, Vector3 attackDir)
    {
        if (hitInfo.transform.TryGetComponent(out HealthComponent health))
            ApplyDamage(health, hitInfo.distance, attackDir);
    }

    protected virtual Vector3 CalculateSpread()
    {
        return new Vector3(
            Random.Range(-_spread.x, _spread.x),
            Random.Range(-_spread.y, _spread.y),
            Random.Range(-_spread.z, _spread.z)
            );
    }

}
