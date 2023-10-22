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
    Rigidbody rb;


    [Header("Movement/Sprint")]
    private float _moveSpeed;
    [SerializeField, Tooltip("Speed at which the player will move")] private float _walkSpeed;
    [SerializeField, Tooltip("Speed at which the player will run")] private float _sprintSpeed;
    [SerializeField, Tooltip("Reference to the transform that determines the orientation of the player")] private Transform _lookOrientation;
    private Transform _orientation;
    [SerializeField, Tooltip("")] private float _groundDrag;
    bool _grounded;

    public Vector2 _moveAxis;
    Vector3 moveDirection;

    [Header("Jump")]
    [SerializeField, Tooltip("")] private float _jumpForce;
    [SerializeField, Tooltip("")] private float _jumpCooldown;
    [SerializeField, Tooltip("")] private float _airMultiplier;

    [Header("Crouching")]

    [SerializeField, Tooltip("Speed at which the player will crouch")] private float _crouchSpeed;
    [SerializeField, Tooltip("")] private float _crouchYScale;
    private float _startYScale;



    //private Vector3 _smoothJumpVel;
    //public float duration = 3;


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
    }
    public void OnSprint(InputAction.CallbackContext ctx)
    {
        if (ctx.performed /*&& _grounded*/)
        {
            _moveSpeed = _sprintSpeed;
        }
        else
        {
            _moveSpeed = _walkSpeed;
        }
    }

    public void OnCrouch(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            StartCoroutine(CrouchSmoothly(_crouchYScale, _crouchSpeed)); //FLIPA SERGI COMO APLICO ESTA MIERDA CHAVAL
        }
        else
        {
            StartCoroutine(CrouchSmoothly(_startYScale, _walkSpeed));
        }
    }

    //---------------EVENT FUNCTIONS---------------//
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        _moveSpeed = _walkSpeed;
        _startYScale = transform.localScale.y;

        _orientation = _lookOrientation;
        _orientation.rotation = Quaternion.Euler(new Vector3(0, _lookOrientation.rotation.y, 0));

    }
    private void Update()
    {
        _orientation = _lookOrientation;
        _orientation.rotation = Quaternion.Euler(new Vector3(0, _lookOrientation.rotation.y, 0));


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

        //ESTO ES PARA EVITAR QUE SE DESLICE
        if (_grounded)
        {
            Vector3 vel = new Vector3(moveDirection.x * _moveSpeed * 1f, rb.velocity.y, moveDirection.z * _moveSpeed * 1f);
            rb.velocity = vel;

            //rb.AddForce(moveDirection.normalized * _moveSpeed * 10f, ForceMode.Force); ///ESTO ES LO DE ANTES
            /*if (_moveAxis.magnitude == 0 ) {
                StartCoroutine(LerpSlide());
            }
            */
        }
        else if (!_grounded)
        {
            rb.AddForce(moveDirection.normalized * _moveSpeed * 10f * _airMultiplier, ForceMode.Force);
            //_smoothJumpVel = rb.velocity;
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > _moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * _moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x * 0.2f, 0f, rb.velocity.z * 0.2f);
        rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
        Physics.gravity = new Vector3(0, -15, 0);
    }

    //DEJAR APARTE

    /*IEnumerator LerpSlide() {
        float timeElapsed = 0;
        while (timeElapsed < duration) {
            Debug.Log("ES ESTE: " + rb.velocity);
            float t = timeElapsed / duration;
            rb.velocity = new Vector3(Mathf.Lerp(_smoothJumpVel.x, 0f, t), rb.velocity.y, Mathf.Lerp(_smoothJumpVel.z,0f, t));
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
    */


    private IEnumerator CrouchSmoothly(float targetYScale, float targetMoveSpeed)
    {
        float duration = 0.2f;  // DURACION DE LA TRANSICION
        Vector3 startScale = transform.localScale;
        float startTime = Time.time;

        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;
            transform.localScale = Vector3.Lerp(startScale, new Vector3(startScale.x, targetYScale, startScale.z), t);
            _moveSpeed = Mathf.Lerp(_moveSpeed, targetMoveSpeed, t);
            yield return null;
        }


        transform.localScale = new Vector3(startScale.x, targetYScale, startScale.z); //(RIGID BODY.SCALE/2)
        _moveSpeed = targetMoveSpeed; //CROUCHING VELOCITY
    }

}
