using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponManager : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private Transform _weaponParent;
    [SerializeField] private Camera _viewCamera;
    [SerializeField] private Transform _lookOrientation;

    [Space(10)]
    //[HideInInspector] 
    public Transform _weaponPivot;
    //[HideInInspector] 
    public Transform _weaponSwayPivot;
    //[HideInInspector] 
    public Transform _weaponRecoilPosition;
    // [HideInInspector] 
    public Transform _weaponRotationPoint;
    //[HideInInspector] 
    public WeaponScriptable _activeWeapon;
    [Space(10)]

    [Header("Aiming parameters")]
    [HideInInspector] private Transform _weaponSights;
    [SerializeField] public bool _isAiming;
    [SerializeField] private float _sightsOffset;
    [SerializeField] private float _sightsOffsetDown;
    [SerializeField] private float _aimingTime;
    [Range(0, 100), SerializeField] private float _aimingFOV;

    private float _normalFOV;
    private Vector3 _weaponAimPositionVelocity;
    private bool _attackHeld = false;

    /// <summary>
    /// Param1: AmmoData of the weapon being swapped IN
    /// </summary>
    public static event Action<AmmoData> OnWeaponSwap;

    private void Awake()
    {
        _normalFOV = _viewCamera.fieldOfView;
    }

    private void Start()
    {
        WeaponScriptable weapon = PlayerInventory.Instance.GetWeapon("Pistol");
        if (weapon == null)
        {
            Debug.LogError($"No WeaponScriptable found for WeaponType:{weapon}");
            return;
        }

        //muy provisional
        _activeWeapon = weapon;

        weapon.SwapIn(_weaponParent, _lookOrientation, this);
        WeaponSwap();
    }

    public void OnFire(InputAction.CallbackContext ctx) //clic izquierdo
    {
        if (ctx.performed)
        {
            _attackHeld = true;
            _activeWeapon.Attack(false);
        }
        else if (ctx.canceled) _attackHeld = false;
    }

    private void Update()
    {
        if (_attackHeld)
        {
            _activeWeapon.Attack(true);
        }

        CalculateAiming();
    }

    void CalculateAiming() //esto me ha costado el triple de lo que deberia haberme costado, no pretendais entenderlo, ni yo lo entiendo
    {
        Vector3 targetPosition = transform.TransformPoint(_activeWeapon._spawnPoint);
        Vector3 targetRotation = _activeWeapon._spawnRotation;
        float currentFOV = _viewCamera.fieldOfView;
        float targetFOV = _normalFOV;

        if (_isAiming)
        {
            targetPosition = _lookOrientation.position + (_weaponPivot.position - _weaponSights.position) + (_lookOrientation.forward * _sightsOffset);
            targetRotation = Vector3.zero;
            targetFOV = _aimingFOV;
        }

        Vector3 aux = _weaponPivot.position;
        aux = Vector3.SmoothDamp(aux, targetPosition, ref _weaponAimPositionVelocity, _aimingTime);
        currentFOV = Mathf.LerpAngle(currentFOV, targetFOV, Time.deltaTime * 10);
        _viewCamera.fieldOfView = currentFOV;

        _weaponPivot.position = aux;
        _weaponPivot.localRotation = Quaternion.Euler(targetRotation);
    }

    public void OnAim(InputAction.CallbackContext ctx) //clic derecho
    {
        if (ctx.performed) _isAiming = true;
        else if (ctx.canceled) _isAiming = false;
    }

    public void OnReload(InputAction.CallbackContext ctx) //R
    {
        _activeWeapon.Reload();
    }

    public void OnSwapNextWeapon(InputAction.CallbackContext ctx) //ruedecilla raton abajo
    {

    }

    public void WeaponSwap()
    {
        Transform[] aux = _weaponParent.GetComponentsInChildren<Transform>(); //una chapuza pero no hay otra forma de hacerlo

        //no se si el orden este se respeta, pero por ejemplo en el cuchillo que no va a tener efecto de particula ni mierda de _weapon sights esto va a petar como un puto campeon
        //ya lo cambiare para entonces para que sea una cerca mirando si es null yy ya veremos si es muy poco eficiente y si hay lagazos al cambiar de arma, que va a ser un problema gordo de cojones
        _weaponPivot = aux[1];
        _weaponSwayPivot = aux[2];
        _weaponRecoilPosition = aux[4];
        _weaponRotationPoint = aux[5];
        _weaponSights = aux[10];

        //_weaponSights = _weaponSwayPivot.GetComponentsInChildren<Transform>().FirstOrDefault(w => w.name == "AimSights");
        //_weaponPivot = aux.FirstOrDefault(w => w.name == "WeaponSwayPivot");

        OnWeaponSwap?.Invoke(_activeWeapon._reloadConfig._ammo); //provisional (no sera provisional vereis pq me va a dar pereza cambiarlo)
    }

    public void OnSwapPrevWeapon(InputAction.CallbackContext ctx) //ruedecilla raton arriba
    {

    }
}
