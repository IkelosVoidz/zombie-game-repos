using UnityEngine;

public class PlayerSwayAndBob : MonoBehaviour
{
    [SerializeField] private PlayerMovement _movementReference;
    [SerializeField] private PlayerCam _camReference;
    private Transform _pivot;
    private Vector3 _initalPosition;

    private Vector2 _move;
    private Vector2 _look;

    [Header("Sway Position")]
    [SerializeField] private float _step = 0.01f;
    [SerializeField] private float _maxStepDistance = 0.06f;
    private Vector3 _swayPos;

    [Header("Sway Rotation")]
    [SerializeField] private float _rotationStep = 4f;
    [SerializeField] private float _maxRotationStep = 5f;
    private Vector3 _swayEulerRot;

    [Header("Sway Smoothing")]
    [SerializeField] float _smoothPos = 10f;
    [SerializeField] float _smoothRot = 12f;


    private void OnEnable()
    {
        PlayerWeaponManager.OnWeaponSwap += WeaponSwapped;
    }

    private void WeaponSwapped(AmmoData obj)
    {
        Transform[] aux = GetComponentsInChildren<Transform>();
        _pivot = aux[1];
        _initalPosition = _pivot.localPosition;
        Debug.Log(_pivot.name);
        Debug.Log(_pivot.localPosition);
    }

    private void OnDisable()
    {
        PlayerWeaponManager.OnWeaponSwap -= WeaponSwapped;
    }

    private void Update()
    {
        GetInput();
        SwayPosition();
        SwayRotation();



        CompositePositionRotation();
    }

    private void GetInput()
    {
        _move = _movementReference._moveAxis;
        _look = _camReference.look;
    }

    void SwayPosition()
    {
        Vector3 invertLook = _look * -_step;
        invertLook.x = Mathf.Clamp(invertLook.x, -_maxStepDistance, _maxStepDistance);
        invertLook.y = Mathf.Clamp(invertLook.y, -_maxStepDistance, _maxStepDistance);
        _swayPos = invertLook;
    }

    void SwayRotation()
    {
        Vector2 invertLook = _look * -_rotationStep;
        invertLook.x = Mathf.Clamp(invertLook.x, -_maxRotationStep, _maxRotationStep);
        invertLook.y = Mathf.Clamp(invertLook.y, -_maxRotationStep, _maxRotationStep);

        _swayEulerRot = new Vector3(invertLook.y, invertLook.x, invertLook.x);

    }

    void CompositePositionRotation()
    {
        Debug.Log(_swayPos);
        Debug.Log(_pivot.localPosition);
        //position
        _pivot.localPosition = Vector3.Lerp(_initalPosition, _initalPosition + _swayPos, Time.deltaTime * _smoothPos);

        //rotation
        _pivot.localRotation = Quaternion.Slerp(_pivot.localRotation, Quaternion.Euler(_swayEulerRot), Time.deltaTime * _smoothRot);
    }
}
