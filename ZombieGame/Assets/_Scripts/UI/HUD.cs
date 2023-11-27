using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] TMP_Text bulletText, healthText;

    private void OnEnable()
    {
        AttackConfigScriptable.OnShoot += UpdateAmmoDisplay;
        ReloadConfigScriptable.OnReloadEnd += UpdateAmmoDisplay;
        PlayerWeaponManager.OnWeaponSwap += UpdateAmmoDisplay;
    }

    private void OnDisable()
    {
        AttackConfigScriptable.OnShoot -= UpdateAmmoDisplay;
        ReloadConfigScriptable.OnReloadEnd -= UpdateAmmoDisplay;
        PlayerWeaponManager.OnWeaponSwap -= UpdateAmmoDisplay;
    }


    public void onHealthChanged(int newHealth, Vector3 attackDirection)
    {
        healthText.text = $"Health: {newHealth}";
    }

    public void UpdateAmmoDisplay(AmmoData ammo)
    {
        bulletText.text = $"{ammo._currentMagAmmo} / {ammo._currentAmmo}";
    }
}
