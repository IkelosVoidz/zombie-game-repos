using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] TMP_Text bulletText, healthText, missionText;

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

    private void Awake()
    {
        missionText.text = "";
    }


    public void onHealthChanged(int newHealth, Vector3 attackDirection)
    {
        healthText.text = $"Health: {newHealth}";
    }

    public void UpdateAmmoDisplay(AmmoData ammo)
    {
        bulletText.text = $"{ammo._currentMagAmmo} / {ammo._currentAmmo}";
    }

    public void InitializeMissions(Dictionary<string, SortedList<int, MissionObjective>> missions)
    {
        ChangeMissionDescription(missions["Inicial"][0]);
    }

    public void ChangeMissionDescription(MissionObjective obj)
    {
        missionText.text = $"{obj._description}";
    }
}
