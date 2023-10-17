using UnityEditor;
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;


/// <summary>
/// This script controls only the movement actions of the player (_moveAxis, look , jump, crouch(si lo acabamos implementando), etc)
/// </summary>
[RequireComponent(typeof(Rigidbody))] //esto esta bien ponerlo , asi si quitamos algo por error no nos va a dejar 
[RequireComponent(typeof(InputSystem))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField, Tooltip("Speed at which the player will move")] private float _moveSpeed;
    [SerializeField, Tooltip("Reference to the transform that determines the orientation of the player")] private Transform _orientation;
    private Vector2 _moveAxis;
    Vector3 moveDirection;

    [Header("Jump")]
    [SerializeField, Tooltip("")] private float _jumpForce;
    [SerializeField, Tooltip("")] private float _jumpCooldown;
    [SerializeField, Tooltip("")] private float _airMultiplier;

    [SerializeField, Tooltip("")] private float _groundDrag;
    [SerializeField, Tooltip("")] private float _playerHeight;
    //[SerializeField, Tooltip("")] private LayerMask whatIsGround;
    bool _grounded;
    //private Vector3 _smoothJumpVel;
    Rigidbody rb;
    //public float duration = 3;
    public void OnMove(InputAction.CallbackContext ctx)
    {
        _moveAxis = ctx.ReadValue<Vector2>();
    }
    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && _grounded) //buena esa ramon asi me gusta aprendiendo de nuestros errores 
        {
            Jump();
        }
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

    }
    private void Update()
    {
        _grounded = Physics.Raycast(transform.position,Vector3.down,_playerHeight*0.5f+0.2f); //SI ESTOY EN EL AIRE grunded = FALSE
        SpeedControl();
        if (_grounded)
            rb.drag = _groundDrag;
        else
            rb.drag = 0;

    }
    private void FixedUpdate()
    {
        MovePlayer();
    }
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

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > _moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * _moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    //ACABAR
    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x * 0.2f, 0f, rb.velocity.z * 0.2f);
        rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
        Physics.gravity = new Vector3(0, -15, 0);
    }
}
