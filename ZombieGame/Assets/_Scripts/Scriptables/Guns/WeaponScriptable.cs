using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// Very loosely based on LLamAcademy's "ScriptableObject-Based Gun system on unity" series 
/// <br></br>
/// Heavily modified to fit our project 
/// </summary>
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

    [SerializeField] private AttackConfigScriptable[] _attackConfigs; //por si queremos hacer una arma que pueda hacer burst y full auto o algo asi
    private AttackConfigScriptable _currentAttackConfig;
    [SerializeField] private DamageConfigScriptable _damageConfig;
    [Tooltip("Only used if weapon is hitscan")]
    [SerializeField] private TrailConfigScriptable _trailConfig;


    //private variables
    private MonoBehaviour _activeMonoBehaviour;
    private GameObject _weaponModel;
    private Transform _lookOrientation;
    /// <summary>
    /// Position of where the bullets should come out, also the ParticleSystem responsible of creating the muzzle flash
    /// </summary>
    private ParticleSystem _gunTip;
    private ObjectPool<TrailRenderer> _trailPool; //to handle destruction of trails to avoid performance problems

    public void SwapIn(Transform parent, Transform lookOrientation, MonoBehaviour activeMonoBehaviour)
    {
        //habra que hacer animacion y tal aqui pero ya se hara
        _lookOrientation = lookOrientation;
        _activeMonoBehaviour = activeMonoBehaviour;
        //dinamico, no nos hace falta crear todas las armas en la escena, si queremos crear mas armas por alguna razon sera facilisimo
        _weaponModel = Instantiate(_modelPrefab);
        _weaponModel.transform.SetParent(parent, false);
        _weaponModel.transform.SetLocalPositionAndRotation(_spawnPoint, Quaternion.Euler(_spawnRotation));

        _gunTip = _weaponModel.GetComponentInChildren<ParticleSystem>();

        foreach (AttackConfigScriptable attackCfg in _attackConfigs)
        {
            attackCfg.InitializeAttackConfig(_lookOrientation, _gunTip, _damageConfig);
        }

        _currentAttackConfig = _attackConfigs[0]; //siempre la primera 
        _currentAttackConfig._lastAttackTime = 0;
        //_trailPool = new ObjectPool<TrailRenderer>(CreateTrail); //por hacer
    }
    public void SwapOut()
    {
        //delete weapon and other shit
    }

    public void changeAttackConfig()
    {
        if (_attackConfigs.Length < 2) return;

        //switch attack config
    }

    //encapsulate in AttackConfig children
    public void Attack(bool inputHeld)
    {
        _currentAttackConfig.Attack(inputHeld);
    }

    /*
    // no me gusta esto no lo borreis porque voy a ir copiando cosas
    private IEnumerator PlayTrail(Vector3 startPoint, Vector3 endPoint, RaycastHit hit)
    {
        TrailRenderer instance = _trailPool.Get();
        instance.gameObject.SetActive(true);
        instance.transform.position = startPoint;
        yield return null;

        instance.emitting = true;
        float distance = Vector3.Distance(startPoint, endPoint);
        float remainingDistance = distance;
        while (remainingDistance > 0)
        {
            instance.transform.position = Vector3.Lerp(
                startPoint, endPoint,
                Mathf.Clamp01(1 - (remainingDistance / distance)));
            remainingDistance -= _trailConfig._simulationSpeed * Time.deltaTime;
            yield return null;

        }

        instance.transform.position = endPoint;

        if (hit.collider != null)
        {
            //cositas!!!
            Debug.Log("HIT!!!");
        }

        yield return new WaitForSeconds(_trailConfig._duration);
        yield return null;
        instance.emitting = false;
        instance.gameObject.SetActive(false);
        _trailPool.Release(instance);
    }


    //encapsulate in TrailConfig
    private TrailRenderer CreateTrail()
    {
        GameObject instance = new GameObject("Bullet Trail");
        TrailRenderer trail = instance.AddComponent<TrailRenderer>();
        trail.colorGradient = _trailConfig._color;
        trail.material = _trailConfig._material;
        trail.widthCurve = _trailConfig._widthCurve;
        trail.time = _trailConfig._duration;
        trail.minVertexDistance = _trailConfig._minVertexDistance;

        trail.emitting = false;
        trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        return trail;
    }*/


}
