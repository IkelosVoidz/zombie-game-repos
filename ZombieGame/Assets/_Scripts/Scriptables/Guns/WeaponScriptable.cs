using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Weapon", order = 0)]
public class WeaponScriptable : ScriptableObject
{
    [Header("Weapon type properties")]
    public WeaponType _type;
    public string _name;
    public GameObject _modelPrefab;

    [Header("Weapon transform properties")]
    public Vector3 _spawnPoint;
    public Vector3 _spawnRotation;

    [Header("Weapon configurations")]

    public AttackConfigScriptable _attackConfig;
    [Tooltip("Only used if weapon is hitscan")]
    public TrailConfigScriptable _trailConfig;


    //private variables
    private MonoBehaviour _activeMonoBehaviour;
    private GameObject _weaponModel;
    private float _lastShootTime;
    private ParticleSystem _shootSystem; //also the gun barrel/tip
    private ObjectPool<TrailRenderer> _trailPool; //to handle destruction of trails to avoid performance problems



}
