using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponManager : MonoBehaviour
{
    [SerializeField]
    private Transform _weaponParent;
    public Transform _weaponPivot;
    public Transform _weaponSwayPivot;
    private Vector3 _weaponInitialPosition;

    public WeaponScriptable _activeWeapon;
    [SerializeField] private Transform _lookOrientation;


    [Header("Aiming parameters")]
    public Transform _weaponSights;
    public bool _isAiming;
    public float _sightsOffset;
    public float _sightsOffsetDown;
    public float _aimingTime;
    private Vector3 _weaponSwayPosition;
    private Vector3 _weaponSwayPositionVelocity;



    private bool _attackHeld = false;

    /// <summary>
    /// Param1: AmmoData of the weapon being swapped IN
    /// </summary>
    public static event Action<AmmoData> OnWeaponSwap;

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

        if (_isAiming)
        {
            targetPosition = _lookOrientation.position + (_weaponPivot.position - _weaponSights.position) + (_lookOrientation.forward * _sightsOffset);
            targetRotation = Vector3.zero;
        }

        Vector3 aux = _weaponPivot.position;
        aux = Vector3.SmoothDamp(aux, targetPosition, ref _weaponSwayPositionVelocity, _aimingTime);

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
        _weaponSights = aux[7];

        //_weaponSights = _weaponSwayPivot.GetComponentsInChildren<Transform>().FirstOrDefault(w => w.name == "AimSights");
        //_weaponPivot = aux.FirstOrDefault(w => w.name == "WeaponSwayPivot");
        _weaponInitialPosition = _weaponPivot.position;

        OnWeaponSwap?.Invoke(_activeWeapon._reloadConfig._ammo); //provisional (no sera provisional vereis pq me va a dar pereza cambiarlo)
    }

    public void OnSwapPrevWeapon(InputAction.CallbackContext ctx) //ruedecilla raton arriba
    {

    }
}
