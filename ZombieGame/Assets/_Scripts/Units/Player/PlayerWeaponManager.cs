using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponManager : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private Transform _weaponParent;
    [SerializeField] private Camera _viewCamera;
    [SerializeField] private Camera _outlineCamera;
    [SerializeField] private Transform _lookOrientation;
    [SerializeField] private PlayerMovement _movement;

    [Space(10)]
    [Header("Pivot References")]
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
    //[HideInInspector] 
    public Animator _weaponAnimator;
    [Space(10)]

    [Header("Aiming parameters")]
    [HideInInspector] private Transform _weaponSights;
    [SerializeField] public bool IsAiming { get; private set; }
    [SerializeField] private float _sightsOffset;
    [SerializeField] private float _sightsOffsetDown;
    [SerializeField] private float _aimingTime;
    [Range(0, 100), SerializeField] private float _aimingFOV;

    private float _normalFOV;
    private Vector3 _weaponAimPositionVelocity;
    private bool _attackHeld = false;
    [HideInInspector] public bool IsReloading { get; private set; } = false;
    [HideInInspector] public bool IsMeleeing { get; private set; } = false;
    [Space(10)]
    [Header("Loadout")]
    [SerializeField] private WeaponScriptable _primary;
    [SerializeField] private WeaponScriptable _secondary;


    /// <summary>
    /// Param1: AmmoData of the weapon being swapped IN
    /// </summary>
    public static event Action<AmmoData> OnWeaponSwap;

    private void Awake()
    {
        _normalFOV = _viewCamera.fieldOfView;
    }

    private void OnEnable()
    {
        GunAnimEvents.OnReloadAnimEnd += OnReloadEnd;
        GunAnimEvents.OnMeleeAnimHit += OnMeleeHit;
        GunAnimEvents.OnSwapAnimEnd += OnSwapOutAnimEnd;
    }

    private void OnDisable()
    {
        GunAnimEvents.OnReloadAnimEnd -= OnReloadEnd;
        GunAnimEvents.OnMeleeAnimHit -= OnMeleeHit;
        GunAnimEvents.OnSwapAnimEnd += OnSwapOutAnimEnd;
    }

    private void Start()
    {
        _primary = PlayerInventory.Instance.GetWeapon(_primary._name);
        _secondary = PlayerInventory.Instance.GetWeapon(_secondary._name);
        if (_primary == null)
        {
            Debug.LogError($"No WeaponScriptable found for WeaponType:{_primary}");
            return;
        }
        if (_secondary == null)
        {
            Debug.LogError($"No WeaponScriptable found for WeaponType:{_secondary}");
            return;
        }

        //muy provisional
        _primary.SwapIn(_weaponParent, _lookOrientation, this, ref _weaponPivot);
        _activeWeapon = _primary;
        WeaponSwap();
    }

    public void OnFire(InputAction.CallbackContext ctx) //clic izquierdo
    {
        if (!IsReloading && !IsMeleeing) //&& !_movement.IsSprinting)
        {
            if (ctx.performed)
            {
                _attackHeld = true;
                _activeWeapon.Attack(false);
            }
            else if (ctx.canceled) _attackHeld = false;
        }
        else _attackHeld = false;
    }

    private void Update()
    {
        if (_attackHeld)
        {
            _activeWeapon.Attack(true);
        }

        CalculateAiming();
    }

    public void CancelAiming()
    {
        //perhaps i should add a can aim false here
        IsAiming = false;
    }

    void CalculateAiming() //esto me ha costado el triple de lo que deberia haberme costado, no pretendais entenderlo, ni yo lo entiendo
    {
        Vector3 targetPosition = transform.TransformPoint(_activeWeapon._spawnPoint);
        Vector3 targetRotation = _activeWeapon._spawnRotation;
        float currentFOV = _viewCamera.fieldOfView;
        float targetFOV = _normalFOV;

        if (IsAiming)
        {
            targetPosition = _lookOrientation.position + (_weaponPivot.position - _weaponSights.position) + (_lookOrientation.forward * _sightsOffset);
            targetRotation = Vector3.zero;
            targetFOV = _aimingFOV;
        }

        Vector3 aux = _weaponPivot.position;
        aux = Vector3.SmoothDamp(aux, targetPosition, ref _weaponAimPositionVelocity, _aimingTime);
        currentFOV = Mathf.LerpAngle(currentFOV, targetFOV, Time.deltaTime * 10);
        _viewCamera.fieldOfView = currentFOV;
        _outlineCamera.fieldOfView = currentFOV;

        _weaponPivot.position = aux;
        _weaponPivot.localRotation = Quaternion.Euler(targetRotation);

        //Vector3(0.165000007,-0.27700001,0.126000002)
        //Vector3(359.923187,353.601105,358.619263)

        //Vector3(0.0149999997,-0.057,-0.123000003) R
        //Vector3(1.06721707e-07,182.475143,50.9622498)

        //Vector3(-0.0936999992,-0.0460000001,0.221799999) L 
        //Vector3(22.9388847,257.630127,240.924545)
    }

    public void OnAim(InputAction.CallbackContext ctx) //clic derecho
    {
        if (!IsReloading && !_movement.IsSprinting && !IsMeleeing)
        {
            if (ctx.performed) IsAiming = true;
            else if (ctx.canceled) IsAiming = false;
        }
    }

    public void OnReload(InputAction.CallbackContext ctx) //R
    {
        if (ctx.performed)
        {
            if (_activeWeapon.CanReload() && !IsReloading && !IsMeleeing)
            {
                IsAiming = false;
                IsReloading = true;
                _weaponAnimator.SetTrigger("Reload");
            }
        }
    }

    public void OnMelee(InputAction.CallbackContext ctx) // V pero se va a cambiar
    {
        if (ctx.performed)
        {
            if (!IsReloading && !IsMeleeing)
            {
                CancelAiming();
                IsMeleeing = true;
                _weaponAnimator.SetTrigger("Melee");

            }
        }
    }

    void OnMeleeHit()
    {
        Debug.Log("MELEE");

        if (Physics.Raycast(
            _lookOrientation.position,
            _lookOrientation.forward,
             out RaycastHit hit,
             float.MaxValue
            ))
        {
            if (hit.transform.TryGetComponent(out HealthComponent health))
            {
                health.TakeDamage(5, _lookOrientation.forward);
            }
        }
    }

    void OnMeleeEnd()
    {
        Debug.Log("MELEE END");
        IsMeleeing = false;
    }

    void OnReloadEnd()
    {
        _activeWeapon.Reload();
        IsReloading = false;
    }

    public void OnSwapNextWeapon(InputAction.CallbackContext ctx) //ruedecilla raton abajo
    {

    }

    public void WeaponSwap()
    {
        IsReloading = false;
        Transform[] aux; //= _weaponParent.GetComponentsInChildren<Transform>(); //una chapuza pero no hay otra forma de hacerlo
                         //_weaponPivot = aux.FirstOrDefault(w => w.name == "WeaponParent");
        _weaponAnimator = _weaponPivot.GetComponentInChildren<Animator>();
        aux = _weaponPivot.GetComponentsInChildren<Transform>();
        _weaponSwayPivot = aux[1];
        for (int i = 3; i < aux.Length; i++)
        {
            if (aux[i].name == "WeaponPosition") _weaponRecoilPosition = aux[i];
            else if (aux[i].name == "RotationPoint") _weaponRotationPoint = aux[i];
            else if (aux[i].name == "AimSights") _weaponSights = aux[i];
        }

        //no se si el orden este se respeta, pero por ejemplo en el cuchillo que no va a tener efecto de particula ni mierda de _weapon sights esto va a petar como un puto campeon
        //ya lo cambiare para entonces para que sea una cerca mirando si es null yy ya veremos si es muy poco eficiente y si hay lagazos al cambiar de arma, que va a ser un problema gordo de cojones
        //_weaponPivot = aux[1];


        OnWeaponSwap?.Invoke(_activeWeapon._reloadConfig._ammo); //provisional (no sera provisional vereis pq me va a dar pereza cambiarlo)
    }

    public void OnSwapPrevWeapon(InputAction.CallbackContext ctx) //ruedecilla raton arriba
    {

    }

    public void OnSwapToPrimary(InputAction.CallbackContext ctx)
    {
        if (_activeWeapon != _primary && !IsReloading && !IsMeleeing)
        {
            CancelAiming();
            _weaponAnimator.SetTrigger("Swap"); //swaps out current weapon 
        }
    }
    public void OnSwapToSecondary(InputAction.CallbackContext ctx)
    {
        if (_activeWeapon != _secondary && !IsReloading && !IsMeleeing)
        {
            CancelAiming();
            _weaponAnimator.SetTrigger("Swap"); //swaps out current weapon 
        }
    }

    public void OnSwapOutAnimEnd()
    {
        if (_activeWeapon == _primary)
        {
            _activeWeapon.SwapOut();
            _secondary.SwapIn(_weaponParent, _lookOrientation, this, ref _weaponPivot);
            _activeWeapon = _secondary;
            WeaponSwap(); //will play swap in animation
        }
        else if (_activeWeapon == _secondary)
        {
            _activeWeapon.SwapOut();
            _primary.SwapIn(_weaponParent, _lookOrientation, this, ref _weaponPivot);
            _activeWeapon = _primary;
            WeaponSwap(); //will play swap in animation
        }
    }
}
