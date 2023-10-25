using UnityEngine;

[CreateAssetMenu(fileName = "Attack Config", menuName = "Weapons/Attack Configuration", order = 2)]
public class AttackConfigScriptable : ScriptableObject
{
    public AttackType _attackType;
    public LayerMask _hitMask;
    public Vector3 _spread = new Vector3(0.1f, 0.1f, 0.1f);
    public float _fireRate = 0.25f;
    public int _bulletsFired = 1; //esto probablemente no lo acabe haciendo asi pero es para tenerlo en mente 
}

public enum AttackType
{
    SemiAuto,
    FullAuto,
    Burst,
    Projectile,
    Melee
}
