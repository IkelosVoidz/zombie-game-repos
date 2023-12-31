using UnityEngine;

public class ProceduralWeaponRecoil : MonoBehaviour
{
    [Header("Reference Points")]
    public Transform recoilPosition;
    public Transform rotationPoint;
    [Space(10)]

    Vector3 rotationalRecoil;
    Vector3 positionalRecoil;
    Vector3 Rot;
    [Header("State:")]
    public bool aiming;

    [Header("Script References")]
    [SerializeField] private PlayerWeaponManager _weaponManager;
    RecoilStatsConfigScriptable _recoilStats;

    private void OnEnable()
    {
        AttackConfigScriptable.OnShoot += Fire;
        PlayerWeaponManager.OnWeaponSwap += WeaponSwapped;
    }

    private void WeaponSwapped(AmmoData obj)
    {
        recoilPosition = _weaponManager._weaponRecoilPosition;
        rotationPoint = _weaponManager._weaponRotationPoint;
        _recoilStats = _weaponManager._activeWeapon._recoilStatsConfig;
    }

    private void OnDisable()
    {
        AttackConfigScriptable.OnShoot -= Fire;
        PlayerWeaponManager.OnWeaponSwap -= WeaponSwapped;
    }

    private void FixedUpdate()
    {
        rotationalRecoil = Vector3.Lerp(rotationalRecoil, Vector3.zero, _recoilStats.rotationalReturnSpeed * Time.deltaTime);
        positionalRecoil = Vector3.Lerp(positionalRecoil, Vector3.zero, _recoilStats.positionalReturnSpeed * Time.deltaTime);

        recoilPosition.localPosition = Vector3.Slerp(recoilPosition.localPosition, positionalRecoil, _recoilStats.positionalRecoilSpeed * Time.fixedDeltaTime);
        Rot = Vector3.Slerp(Rot, rotationalRecoil, _recoilStats.rotationalRecoilSpeed * Time.fixedDeltaTime);
        rotationPoint.localRotation = Quaternion.Euler(Rot);
    }

    private void Update()
    {
        aiming = _weaponManager.IsAiming;
    }


    public void Fire(AmmoData unused)
    {
        if (aiming)
        {
            rotationalRecoil += new Vector3(-_recoilStats.RecoilRotationAim.x, Random.Range(-_recoilStats.RecoilRotationAim.y, _recoilStats.RecoilRotationAim.y), Random.Range(-_recoilStats.RecoilRotationAim.z, _recoilStats.RecoilRotationAim.z));
            positionalRecoil += new Vector3(Random.Range(-_recoilStats.RecoilKickBackAim.x, _recoilStats.RecoilKickBackAim.x), Random.Range(-_recoilStats.RecoilKickBackAim.y, _recoilStats.RecoilKickBackAim.y), _recoilStats.RecoilKickBackAim.z);

        }
        else
        {
            rotationalRecoil += new Vector3(-_recoilStats.RecoilRotation.x, Random.Range(-_recoilStats.RecoilRotation.y, _recoilStats.RecoilRotation.y), Random.Range(-_recoilStats.RecoilRotation.z, _recoilStats.RecoilRotation.z));
            positionalRecoil += new Vector3(Random.Range(-_recoilStats.RecoilKickBack.x, _recoilStats.RecoilKickBack.x), Random.Range(-_recoilStats.RecoilKickBack.y, _recoilStats.RecoilKickBack.y), _recoilStats.RecoilKickBack.z);
        }
    }
}