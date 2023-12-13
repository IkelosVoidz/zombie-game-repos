using UnityEngine;

public class WeaponSway : MonoBehaviour
{

    [Header("Sway Properties")]
    [SerializeField] private float swayAmount = 0.01f;
    [SerializeField] public float maxSwayAmount = 0.1f;
    [SerializeField] public float swaySmooth = 9f;
    [SerializeField] public AnimationCurve swayCurve;

    [Range(0f, 1f)]
    [SerializeField] public float swaySmoothCounteraction = 1f;

    [Header("Rotation")]
    [SerializeField] public float rotationSwayMultiplier = 1f;

    [Header("Position")]
    [SerializeField] public float positionSwayMultiplier = -1f;


    private Vector2 sway;
    private Transform weaponTransform;


    [Header("Script References")]
    [SerializeField] private PlayerCam _camReference;
    [SerializeField] private PlayerWeaponManager _weaponManager;
    [SerializeField] private PlayerMovement _movement;
    private Vector2 _look;

    private void OnEnable()
    {
        PlayerWeaponManager.OnWeaponSwap += WeaponSwapped;
    }

    private void WeaponSwapped(AmmoData obj)
    {
        weaponTransform = _weaponManager._weaponSwayPivot;
    }

    private void OnDisable()
    {
        PlayerWeaponManager.OnWeaponSwap -= WeaponSwapped;
    }

    private void Reset()
    {
        Keyframe[] ks = new Keyframe[] { new Keyframe(0, 0, 0, 2), new Keyframe(1, 1) };
        swayCurve = new AnimationCurve(ks);
    }

    private Vector3 LissajousCurve(float Time, float A, float B)
    {
        return new Vector3(Mathf.Sin(Time), A * Mathf.Sin(B * Time + Mathf.PI));
    }

    [Header("Breathing Animation")]
    [SerializeField] float _breathingSwayAmountA = 1;
    [SerializeField] float _breathingSwayAmountB = 1;
    [SerializeField] float _breathingSwayScale = 300;
    [SerializeField] float _breathingSwayLerpSpeed = 14;
    private float swayTime;
    private Vector3 swayPosition;
    private void CalculateWeaponBreathing()
    {
        var targetPosition = LissajousCurve(swayTime, _breathingSwayAmountA, _breathingSwayAmountB) / _breathingSwayScale;
        swayTime += Time.deltaTime;
        if (swayTime > 6.3f) swayTime = 0;
        swayPosition = Vector3.Lerp(swayPosition, targetPosition, Time.smoothDeltaTime * _breathingSwayLerpSpeed);

        weaponTransform.localPosition = swayPosition;
    }

    private void CalculateWeaponSway()
    {
        sway = Vector2.MoveTowards(sway, Vector2.zero, swayCurve.Evaluate(Time.deltaTime * swaySmoothCounteraction * sway.magnitude * swaySmooth));
        sway = Vector2.ClampMagnitude(new Vector2(_look.x, _look.y) + sway, maxSwayAmount);

        weaponTransform.localPosition = Vector3.Lerp(weaponTransform.localPosition, new Vector3(sway.x, sway.y, 0) * positionSwayMultiplier, swayCurve.Evaluate(Time.deltaTime * swaySmooth));
        weaponTransform.localRotation = Quaternion.Slerp(weaponTransform.localRotation, Quaternion.Euler(Mathf.Rad2Deg * rotationSwayMultiplier * new Vector3(-sway.y, sway.x, 0)), swayCurve.Evaluate(Time.deltaTime * swaySmooth));
    }

    private void GetInput()
    {
        _look = _camReference.look;
        _look *= swayAmount;
    }


    private void Update()
    {
        GetInput();
        CalculateWeaponSway();
        CalculateWeaponBreathing();
    }
}
