using System;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{

    [Header("Sway Properties")]
    [SerializeField] private float swayAmount = 0.01f;
    [SerializeField] public float maxSwayAmount = 0.1f;
    private float _currentMaxSwayAmount;
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

    [Header("State")]
    [SerializeField] private bool _aiming;

    [Header("Script References")]
    [SerializeField] private PlayerCam _camReference;
    [SerializeField] private PlayerWeaponManager _weaponManager;
    [SerializeField] private PlayerMovement _movement;
    private Vector2 _look;
    private Vector2 _move;


    [SerializeField] private AudioClip[] _leftFootsteps;
    [SerializeField] private AudioClip[] _rightFootsteps;

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
        // we dont want it to sway that much when aiming
        if (_aiming) _currentMaxSwayAmount = maxSwayAmount / 2;
        else _currentMaxSwayAmount = maxSwayAmount;

        sway = Vector2.MoveTowards(sway, Vector2.zero, swayCurve.Evaluate(Time.deltaTime * swaySmoothCounteraction * sway.magnitude * swaySmooth));
        sway = Vector2.ClampMagnitude(new Vector2(_look.x, _look.y) + sway, _currentMaxSwayAmount);

        weaponTransform.localPosition = Vector3.Lerp(weaponTransform.localPosition, (new Vector3(sway.x, sway.y, 0) * positionSwayMultiplier) + bobPosition, swayCurve.Evaluate(Time.deltaTime * swaySmooth));
        weaponTransform.localRotation = Quaternion.Slerp(weaponTransform.localRotation, Quaternion.Euler(Mathf.Rad2Deg * rotationSwayMultiplier * new Vector3(-sway.y, sway.x, 0)) * Quaternion.Euler(bobEulerRotation), swayCurve.Evaluate(Time.deltaTime * swaySmooth));
    }

    private void CompositePositionRotation()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, bobPosition, Time.deltaTime * swaySmooth);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(bobEulerRotation), Time.deltaTime * swaySmooth);
    }

    private void GetInput()
    {
        _look = _camReference.look;
        _look *= swayAmount;
        _move = _movement._moveAxis;
        _move = _move.normalized;
    }


    private void Update()
    {
        _aiming = _weaponManager.IsAiming;
        GetInput();
        BobOffset();
        BobRotation();
        CalculateWeaponSway();
        CompositePositionRotation();
        //if (_move.x != 0 || _move.y != 0) CalculateWeaponBreathing();
    }


    [Header("Bobbing")]
    public float speedCurve;
    float curveSin { get => Mathf.Sin(speedCurve); }
    float curveCos { get => Mathf.Cos(speedCurve); }

    public Vector3 travelLimit = Vector3.one * 0.025f;
    private Vector3 currentTravelLimit;
    public Vector3 bobLimit = Vector3.one * 0.01f;
    Vector3 bobPosition;

    public float bobExaggeration;

    [Header("Bob Rotation")]
    public Vector3 multiplier;
    public Vector3 currentMultiplier;
    Vector3 bobEulerRotation;

    bool left = false;

    void BobOffset()
    {
        if (_aiming) currentTravelLimit = travelLimit / 3;
        else currentTravelLimit = travelLimit;

        // Use the sine wave phase to determine left and right extremes
        float sineWavePhase = Mathf.Sin(speedCurve);

        // Check if the player is on the ground and moving
        if (_movement.IsGrounded && _movement.rb.velocity.magnitude > 0.1f)
        {
            // Trigger left footstep sound when the viewmodel is on the left
            if (sineWavePhase < 0 && !left)
            {
                // Play left footstep sound here
                SoundManager.Instance.PlayRandomSoundFXClip(_leftFootsteps, _movement.transform, 0.2f);
                Debug.Log("Left");
                left = true;
            }

            // Trigger right footstep sound when the viewmodel is on the right
            if (sineWavePhase > 0 && left)
            {
                // Play right footstep sound here
                SoundManager.Instance.PlayRandomSoundFXClip(_rightFootsteps, _movement.transform, 0.2f);
                Debug.Log("Right");
                left = false;
            }
        }


        speedCurve += Time.deltaTime * (_movement.IsGrounded ? _movement.rb.velocity.magnitude : 1f) + 0.01f;

        bobPosition.x = (curveCos * bobLimit.x * (_movement.IsGrounded ? 1 : 0)) - (_move.x * currentTravelLimit.x);
        bobPosition.y = (curveSin * bobLimit.y) - (_movement.rb.velocity.y * currentTravelLimit.y);
        bobPosition.z = -(_move.y * currentTravelLimit.z);
    }

    void BobRotation()
    {
        if (_aiming) currentMultiplier = Vector3.one * 0.1f;
        else currentMultiplier = multiplier;

        bobEulerRotation.x = (_move != Vector2.zero ? currentMultiplier.x * (Mathf.Sin(2 * speedCurve)) : currentMultiplier.x * (Mathf.Sin(2 * speedCurve) / 2));
        bobEulerRotation.y = (_move != Vector2.zero ? currentMultiplier.y * curveCos : 0);
        bobEulerRotation.z = (_move != Vector2.zero ? currentMultiplier.z * curveCos * _move.x : 0);
    }

}
