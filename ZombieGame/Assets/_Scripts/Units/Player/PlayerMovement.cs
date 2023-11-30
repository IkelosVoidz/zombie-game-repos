using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// This script controls only the movement actions of the player (_moveAxis, look , jump, crouch(si lo acabamos implementando), etc)
/// </summary>
[RequireComponent(typeof(Rigidbody))] //esto esta bien ponerlo , asi si quitamos algo por error no nos va a dejar 
[RequireComponent(typeof(InputSystem))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Player")]
    [SerializeField, Tooltip("")] private float _playerHeight;
    [SerializeField, Tooltip("")] private Transform _playerObj;
    Rigidbody rb;


    [Header("Movement/Sprint")]
    private float _moveSpeed;
    [SerializeField, Tooltip("Speed at which the player will move")] private float _walkSpeed;
    [SerializeField, Tooltip("Speed at which the player will run")] private float _sprintSpeed;
    [SerializeField, Tooltip("Reference to the transform that determines the orientation of the player")] private Transform _orientation;
    //private Transform _lookOrientation;
    [SerializeField, Tooltip("")] private float _groundDrag;
    bool _grounded;

    public Vector2 _moveAxis { get; private set; }
    Vector3 moveDirection;

    [Header("Jump")]
    [SerializeField, Tooltip("")] private float _jumpForce;
    [SerializeField, Tooltip("")] private float _jumpCooldown;
    [SerializeField, Tooltip("")] private float _airMultiplier;

    [Header("Crouching")]

    [SerializeField, Tooltip("Speed at which the player will crouch")] private float _crouchSpeed;
    [SerializeField, Tooltip("")] private float _crouchYScale;
    private float _startYScale;

    [Header("Slope Handling")]
    [SerializeField, Tooltip("")] private float _maxSlopeAngle;
    private RaycastHit _slopeHit;
    private bool _exitingSlope;




    public bool _sprinting { get; private set; }

    private Vector3 _airMoveDirection;

    //---------------INPUTS FUNCTIONS---------------//
    public void OnMove(InputAction.CallbackContext ctx)
    {
        _moveAxis = ctx.ReadValue<Vector2>();

    }
    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && _grounded)
        {
            Jump();
        }
        else
        {
            _exitingSlope = false;
        }
    }
    public void OnSprint(InputAction.CallbackContext ctx)
    {

        if (ctx.performed)
        {
            _moveSpeed = _sprintSpeed;
            _sprinting = true;
        }
        else
        {
            _moveSpeed = _walkSpeed;
            _sprinting = false;
        }
    }

    public void OnCrouch(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && !_sprinting)
        {
            StartCoroutine(CrouchSmoothly(_crouchYScale, _crouchSpeed));
        }
        else if (!ctx.performed && !_sprinting) {
            StartCoroutine(CrouchSmoothly(_startYScale, _walkSpeed));
        }
    }

    //---------------EVENT FUNCTIONS---------------//
    private void Start()
    {
        Physics.gravity = new Vector3(0, -15, 0);
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        _moveSpeed = _walkSpeed;
        _startYScale = _playerObj.localScale.y;

    }
    private void Update()
    {

        _grounded = Physics.Raycast(transform.position, Vector3.down, _playerHeight * 0.5f + 0.2f); //SI ESTOY EN EL AIRE grunded = FALSE

        SpeedControl();
        if (_grounded)
        {
            rb.drag = _groundDrag;
        }
        else
            rb.drag = 0;

    }
    private void FixedUpdate()
    {
        MovePlayer();
    }


    //---------------MOVEMENT FUNCTIONS---------------//
    private void MovePlayer()
    {
        moveDirection = _orientation.forward * _moveAxis.y + _orientation.right * _moveAxis.x;
        //bool exitedSlope = false;
        //ESTO ES PARA EVITAR QUE SE DESLICE

        if (OnSlope() && !_exitingSlope)
        {
            Vector3 vel = new Vector3(GetSlopeMoveDirection().x * _moveSpeed * 1f, GetSlopeMoveDirection().y * _moveSpeed * 1f, GetSlopeMoveDirection().z * _moveSpeed * 1f);
            rb.velocity = vel;
            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
 
        }
        else if (_grounded)
        {
            Vector3 vel = new Vector3(moveDirection.x * _moveSpeed * 1f, rb.velocity.y, moveDirection.z * _moveSpeed * 1f);
            rb.velocity = vel;
        }

        else if (!_grounded)
        {
            rb.AddForce(moveDirection.normalized * _moveSpeed * 200f * _airMultiplier, ForceMode.Force); //ARREGLAR
        }

        rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        if (OnSlope() && !_exitingSlope)
        {
            if (rb.velocity.magnitude > _moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * _moveSpeed;
            }
        }
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            if (flatVel.magnitude > _moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * _moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void Jump()
    {
        _exitingSlope = true;

        rb.velocity = new Vector3(rb.velocity.x * 0.2f, 0f, rb.velocity.z * 0.2f);
        rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out _slopeHit, _playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, _slopeHit.normal);
            return angle < _maxSlopeAngle && angle != 0;
        }
        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, _slopeHit.normal).normalized;
    }

    

    private IEnumerator CrouchSmoothly(float targetYScale, float targetMoveSpeed)
    {
        float duration = 0.2f;  // DURACION DE LA TRANSICION
        Vector3 startScale = _playerObj.localScale;
        float startTime = Time.time;

        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;
            rb.AddForce(Vector3.down * 1f, ForceMode.Impulse);
            _playerObj.localScale = Vector3.Lerp(startScale, new Vector3(startScale.x, targetYScale, startScale.z), t);
            _moveSpeed = Mathf.Lerp(_moveSpeed, targetMoveSpeed, t);
            yield return null;
        }

        _playerObj.localScale = new Vector3(startScale.x, targetYScale, startScale.z); //(RIGID BODY.SCALE/2)
        _moveSpeed = targetMoveSpeed; //CROUCHING VELOCITY
    }
}
