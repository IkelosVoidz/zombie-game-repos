using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// Loosely based on LLamAcademy's "ScriptableObject-Based Gun system on unity" series 
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

    public AttackConfigScriptable _attackConfig;
    [Tooltip("Only used if weapon is hitscan")]
    public TrailConfigScriptable _trailConfig;


    //private variables
    private MonoBehaviour _activeMonoBehaviour;
    private GameObject _weaponModel;
    private Transform _lookOrientation;

    //private variables exclusive to weapons that shoot bullets 
    private float _lastAttackTime;
    /// <summary>
    /// Position of where the bullets should come out, also the ParticleSystem responsible of creating the muzzle flash
    /// </summary>
    private ParticleSystem _shootSystem;
    private ObjectPool<TrailRenderer> _trailPool; //to handle destruction of trails to avoid performance problems

    public void SwapIn(Transform parent, Transform lookOrientation, MonoBehaviour activeMonoBehaviour)
    {
        //habra que hacer animacion y tal aqui pero ya se hara
        _lookOrientation = lookOrientation;
        _activeMonoBehaviour = activeMonoBehaviour;
        _lastAttackTime = 0;
        _trailPool = new ObjectPool<TrailRenderer>(CreateTrail);

        //dinamico, no nos hace falta crear todas las armas en la escena, si queremos crear mas armas por alguna razon sera facilisimo
        _weaponModel = Instantiate(_modelPrefab);
        _weaponModel.transform.SetParent(parent, false);
        _weaponModel.transform.SetLocalPositionAndRotation(_spawnPoint, Quaternion.Euler(_spawnRotation));

        _shootSystem = _weaponModel.GetComponentInChildren<ParticleSystem>(); //investigate if there is a better way to get this component
    }
    public void SwapOut()
    {
        //delete weapon and other shit
    }

    //encapsulate in AttackConfig children
    public void Attack()
    {
        if (Time.time > _attackConfig._fireRate + _lastAttackTime)
        {
            _lastAttackTime = Time.time;
            _shootSystem.Play();


            Vector3 shootDirection = _shootSystem.transform.forward
                + new Vector3(
                    Random.Range(-_attackConfig._spread.x, _attackConfig._spread.x),
                    Random.Range(-_attackConfig._spread.y, _attackConfig._spread.y),
                    Random.Range(-_attackConfig._spread.z, _attackConfig._spread.z)
                    );

            shootDirection.Normalize();


            if (Physics.Raycast(
                _shootSystem.transform.position,
                shootDirection,
                 out RaycastHit hit,
                 float.MaxValue,
                 _attackConfig._hitMask
                ))
            {
                _activeMonoBehaviour.StartCoroutine(
                    PlayTrail(_shootSystem.transform.position, hit.point, hit)
                    );
            }
            else
            {
                _activeMonoBehaviour.StartCoroutine(
                    PlayTrail(_shootSystem.transform.position, _shootSystem.transform.position + (shootDirection * _trailConfig._missDistance), new RaycastHit())
                    );
            }
        }
    }

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
    }


}
