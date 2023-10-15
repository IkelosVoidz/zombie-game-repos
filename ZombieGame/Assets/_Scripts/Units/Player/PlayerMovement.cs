using UnityEngine;
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
    bool _readyToJump;

    //[SerializeField, Tooltip("")] private float groundDrag;
    //[SerializeField, Tooltip("")] private float playerHeight;
    //[SerializeField, Tooltip("")] private LayerMask whatIsGround;
    //bool grounded;

    Rigidbody rb;
    public void OnMove(InputAction.CallbackContext ctx)
    {
        _moveAxis = ctx.ReadValue<Vector2>();
    }
    public void OnJump(InputAction.CallbackContext ctx)
    {
        Debug.Log("SALTAR");
        if (ctx.performed) //buena esa ramon asi me gusta aprendiendo de nuestros errores 
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
        //grounded = Physics.Raycast(transform.position,Vector3.down,playerHeight*0.5f+0.2f,whatIsGround);

        SpeedControl();

        /*if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;*/

    }
    private void FixedUpdate()
    {
        MovePlayer();
    }
    private void MovePlayer()
    {
        moveDirection = _orientation.forward * _moveAxis.y + _orientation.right * _moveAxis.x;
        rb.AddForce(moveDirection.normalized * _moveSpeed * 10f, ForceMode.Force);
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

    //ACABAR
    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        _readyToJump = true;
    }
}
