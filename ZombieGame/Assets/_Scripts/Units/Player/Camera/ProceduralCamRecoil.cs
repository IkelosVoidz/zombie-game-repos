using UnityEngine;

public class ProceduralCamRecoil : MonoBehaviour
{
    [Header("Recoil Settings")]
    public float rotationSpeed = 6;
    public float returnSpeed = 25;
    [Space()]

    [Header("Hipfire:")]
    public Vector3 RecoilRotation = new Vector3(2f, 2f, 2f);
    [Space()]

    [Header("Aiming")]
    public Vector3 RecoilRotationAiming = new Vector3(0.5f, 0.5f, 1.5f);
    [Space()]

    [Header("State")]
    public bool aiming;

    [Header("Script References")]
    [SerializeField] private PlayerWeaponManager _weaponManager;

    private Vector3 currentRotation;
    private Vector3 Rot;


    private void OnEnable()
    {
        AttackConfigScriptable.OnShoot += Fire;
    }

    private void OnDisable()
    {
        AttackConfigScriptable.OnShoot -= Fire;
    }

    private void FixedUpdate()
    {
        currentRotation = Vector3.Lerp(currentRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        Rot = Vector3.Slerp(Rot, currentRotation, rotationSpeed * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(Rot);
    }

    void Update()
    {
        aiming = _weaponManager._isAiming;
    }

    public void Fire(AmmoData unused)
    {
        if (aiming)
        {
            currentRotation += new Vector3(-RecoilRotationAiming.x, Random.Range(-RecoilRotationAiming.y, RecoilRotationAiming.y), Random.Range(-RecoilRotationAiming.z, RecoilRotationAiming.z));
        }
        else
        {
            currentRotation += new Vector3(-RecoilRotation.x, Random.Range(-RecoilRotation.y, RecoilRotation.y), Random.Range(-RecoilRotation.z, RecoilRotation.z));
        }
    }
}
