using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponManager : MonoBehaviour
{
    [SerializeField]
    private WeaponType _weapon;
    [SerializeField]
    private Transform _gunParent;
    [SerializeField]
    private List<WeaponScriptable> _weapons; //inventario provisional

    public WeaponScriptable _activeWeapon;
    [SerializeField] private Transform _lookOrientation;

    private bool _attackHeld = false;

    private void Start()
    {

        //probablemente acabe haciendo un string check porque habran multiples armas del mismo tipo supongo , la unica forma de diferenciarlas sera con string
        WeaponScriptable weapon = _weapons.Find(weapon => weapon._type == _weapon); //mega super hiper provisional pero para tener un arma funcional para ver si funciona todo el resto
        if (weapon == null)
        {
            Debug.LogError($"No WeaponScriptable found for WeaponType:{weapon}");
            return;
        }

        //muy provisional
        _activeWeapon = weapon;
        weapon.SwapIn(_gunParent, _lookOrientation, this);
    }

    public void OnFire(InputAction.CallbackContext ctx) //clic izquierdo
    {
        if (ctx.performed)
        {
            _attackHeld = true;
            //_activeWeapon.Attack(false);
        }
        else if (ctx.canceled) _attackHeld = false;
    }

    private void Update()
    {
        if (_attackHeld)
        {
            _activeWeapon.Attack(true);
        }
    }

    public void OnAim(InputAction.CallbackContext ctx) //clic derecho
    {

    }

    public void OnReload(InputAction.CallbackContext ctx) //R
    {
        _activeWeapon.Reload();
        //empezar 
    }

    public void OnSwapNextWeapon(InputAction.CallbackContext ctx) //ruedecilla raton abajo
    {

    }

    public void OnSwapPrevWeapon(InputAction.CallbackContext ctx) //ruedecilla raton arriba
    {

    }
}
