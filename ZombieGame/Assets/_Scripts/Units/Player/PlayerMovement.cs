using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This script controls only the movement actions of the player (move, look , jump, crouch(si lo acabamos implementando), etc)
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject CameraHolder;
    public float speed, sensitivity,maxForce;
    private Vector2 move, look;
    private float lookRotation;

    public void OnMove(InputAction.CallbackContext ctx) { 
        move = ctx.ReadValue<Vector2>(); 
    }
    public void OnLook(InputAction.CallbackContext ctx) {
        look = ctx.ReadValue<Vector2>();
    }

    public void Movement()
    {
        Vector3 currentVelocity = rb.velocity;
        Vector3 targetVelocity = new Vector3(move.x,0,move.y);
        targetVelocity *= speed;

        targetVelocity = transform.TransformDirection(targetVelocity);
        Vector3 velocityChange = (targetVelocity - currentVelocity);

        Vector3.ClampMagnitude(velocityChange,maxForce);

        rb.AddForce(velocityChange,ForceMode.VelocityChange);

    }
    public void Look()
    {
        transform.Rotate(Vector3.up*look.x*sensitivity);
        lookRotation += (-look.y * sensitivity);
        lookRotation = Mathf.Clamp(lookRotation,-90,90);
        CameraHolder.transform.eulerAngles = new Vector3(lookRotation,CameraHolder.transform.eulerAngles.y, CameraHolder.transform.eulerAngles.z);

    }
    public void Jump()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate() //todo lo relacionado con fisicas siempre en fixed update
    {
        Movement();
    }
    private void LateUpdate() //el movimiento de la camara deberia ser aqui, no se porque no me pregunten pero se que es asi
    {
        Look();
    }
}
