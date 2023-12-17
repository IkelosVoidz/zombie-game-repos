using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Ammo Config")]
public class ReloadConfigScriptable : ScriptableObject
{
    [SerializeField] int _maxAmmo;
    [SerializeField] int _magSize;
    [SerializeField] public AmmoData _ammo;
    [SerializeField] float _reloadTime;

    /// <summary>
    /// When the active weapon reloads
    /// </summary>
    public static event Action<AmmoData> OnReloadEnd;


    public void Reload()
    {
        //int maxReloadAmount = Mathf.Min(_magSize, _ammo._currentAmmo);
        //int avaliableBulletsInCurrentMag = _magSize - _ammo._currentMagAmmo;
        //int reloadAmount = Mathf.Min(maxReloadAmount, avaliableBulletsInCurrentMag);

        _ammo._currentMagAmmo = _magSize;
        //_ammo._currentMagAmmo += reloadAmount;
        //_ammo._currentAmmo -= reloadAmount;
        OnReloadEnd?.Invoke(_ammo);
    }

    public bool CanReload()
    {
        return _ammo._currentMagAmmo < _magSize && _ammo._currentAmmo > 0;
    }

    public bool CanShoot()
    {
        return _ammo._currentMagAmmo > 0;
    }

    public void DecreaseAmmo()
    {
        _ammo._currentMagAmmo--;
    }
}

[Serializable]
public struct AmmoData
{
    [SerializeField] public int _currentAmmo;
    [SerializeField] public int _currentMagAmmo;
}
