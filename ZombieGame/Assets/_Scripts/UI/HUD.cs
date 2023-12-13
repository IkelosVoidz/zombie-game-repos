using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] TMP_Text bulletText, healthText, missionText;

    [SerializeField] Image bloodSplatter;

    [Header("Ammo")]
    [SerializeField] TextMeshProUGUI magazineAmmoUI;
    [SerializeField] TextMeshProUGUI totalAmmoUI;
    [SerializeField] Image ammoTypeUI;

    [Header("Weapon")]
    [SerializeField] Image activeWeaponUI;

    [Header("Throwables")]
    [SerializeField] Image lethalUI;
    [SerializeField] TextMeshProUGUI lethalAmountUI;

    [SerializeField] Image tacticalUI;
    [SerializeField] TextMeshProUGUI tacticalAmountUI;


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

        onHealthChanged(90, Vector3.one);
    }


    public void onHealthChanged(int newHealth, Vector3 attackDirection)
    {
        healthText.text = $"Health: {newHealth}";


        newHealth = Mathf.Clamp(newHealth, 0, 100);
        Color auxColor = bloodSplatter.color;


        auxColor.a = -(newHealth / 100f) + 1f;
        bloodSplatter.color = auxColor;
    }

    public void UpdateAmmoDisplay(AmmoData ammo)
    {
        bulletText.text = $"{ammo._currentMagAmmo} / {ammo._currentAmmo}";
        magazineAmmoUI.text = $"{ammo._currentMagAmmo}";
        totalAmmoUI.text = $"{ammo._currentAmmo}";
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
