using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This script controls only the movement actions of the player (move, look , jump, crouch(si lo acabamos implementando), etc)
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;


    //public float groundDrag;

    [Header("Movement")]
    //public float playerHeight;
    //public LayerMask whatIsGround;
    //bool grounded;


    public Transform orientation;
    private Vector2 move;

    Vector3 moveDirection;
    Rigidbody rb;
    public void OnMove(InputAction.CallbackContext ctx)
    {
        move = ctx.ReadValue<Vector2>();
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
        moveDirection = orientation.forward * move.y + orientation.right * move.x;

        rb.AddForce(moveDirection.normalized*moveSpeed*10f, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    //ACABAR
    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x,0f,rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
}
