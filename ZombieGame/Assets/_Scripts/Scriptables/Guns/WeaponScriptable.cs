using System;
using UnityEngine;

/// <summary>
/// Very loosely based on LLamAcademy's "ScriptableObject-Based Gun system on unity" series 
/// <br></br>
/// Heavily modified to fit our project 
/// </summary>
[CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Weapon", order = 0)]
public class WeaponScriptable : InventoryObjectSO
{
    [Header("Weapon type properties")]
    public GameObject _modelPrefab;

    [Header("Weapon transform properties")]
    public Vector3 _spawnPoint;
    public Vector3 _spawnRotation;
    public Vector3 _aimPosition;

    [Header("Weapon configurations")]

    [SerializeField] private AttackConfigScriptable[] _attackConfigs; //por si queremos hacer una arma que pueda hacer burst y full auto o algo asi
    private AttackConfigScriptable _currentAttackConfig;
    [SerializeField] private DamageConfigScriptable _damageConfig;
    [SerializeField] public ReloadConfigScriptable _reloadConfig; //sera privado pero es para probar cosas ahora (se va a quedar asi al final pq que pereza)
    [SerializeField] public RecoilStatsConfigScriptable _recoilStatsConfig;
    [SerializeField] public AudioConfigScriptable _audioConfig;
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


    /// <summary>
    /// When the selected weapon swaps in or out PARAM1 : wether its swaping in or not
    /// </summary>
    public static event Action<bool> OnSwap;

    public void OnValidate()
    {
        _type = "Weapon";
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    public Transform[] SwapIn(Transform parent, Transform lookOrientation, MonoBehaviour activeMonoBehaviour)
    {
        _lookOrientation = lookOrientation;
        _activeMonoBehaviour = activeMonoBehaviour;
        //dinamico, no nos hace falta crear todas las armas en la escena, si queremos crear mas armas por alguna razon sera facilisimo
        _weaponModel = Instantiate(_modelPrefab);
        _weaponModel.SetActive(false); //fix for a bug in which the weapon appears for a single frame on screen before the swap_in animation plays
        _weaponModel.transform.SetParent(parent, false);
        Transform[] aux = _weaponModel.transform.GetComponentsInChildren<Transform>();

        _weaponModel.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        _gunTip = _weaponModel.GetComponentInChildren<ParticleSystem>();

        foreach (AttackConfigScriptable attackCfg in _attackConfigs)
        {
            attackCfg.InitializeAttackConfig(_lookOrientation, _gunTip, _damageConfig, _reloadConfig);
        }

        _currentAttackConfig = _attackConfigs[0]; //siempre la primera 
        _currentAttackConfig._lastAttackTime = 0;
        //_trailPool = new ObjectPool<TrailRenderer>(CreateTrail); //por hacer

        OnSwap?.Invoke(true);
        return aux; //necesitamos el pivot para hacer cosas
    }

    public void SwapOut()
    {
        //delete weapon and other shit
        Destroy(_weaponModel);
        OnSwap?.Invoke(false);
    }

    public void changeAttackConfig()
    {
        if (_attackConfigs.Length < 2) return;

        //switch attack config
    }

    public void Attack(bool inputHeld)
    {
        if (_reloadConfig.CanShoot() && _currentAttackConfig.CanAttack())
        {
            if (inputHeld && !_currentAttackConfig._fullAuto)
            {
                _currentAttackConfig.Attack(inputHeld);
            }
            else if (inputHeld && _currentAttackConfig._fullAuto)
            {
                _currentAttackConfig.Attack(inputHeld);
                SoundManager.Instance.Play2DRandomSoundFXClip(_audioConfig._fireSounds, 0.3f);
            }
            else
            {
                _currentAttackConfig.Attack(inputHeld);
                SoundManager.Instance.Play2DRandomSoundFXClip(_audioConfig._fireSounds, 0.3f);
            }
        }
        else if (!_reloadConfig.CanShoot() && !_currentAttackConfig.CanAttack())
        {
            SoundManager.Instance.Play2DSoundFXClip(_audioConfig._emptyClip, 0.8f);
        }
    }

    public bool CanReload()
    {
        return _reloadConfig.CanReload();
    }

    public void Reload()
    {
        _reloadConfig.Reload();
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
