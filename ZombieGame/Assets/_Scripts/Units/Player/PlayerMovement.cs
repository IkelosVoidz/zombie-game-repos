using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This script controls only the movement actions of the player (move, look , jump, crouch(si lo acabamos implementando), etc)
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    
    private Vector2 _moveAxis, _lookAxis;

    public void OnMove(InputAction.CallbackContext ctx) { _moveAxis = ctx.ReadValue<Vector2>(); }
    public void OnLook(InputAction.CallbackContext ctx) { _lookAxis = ctx.ReadValue<Vector2>(); }

    public void Movement()
    {
        //buenas tardes roman
    }
    public void Look()
    {
      
    }
    public void Jump()
    {

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void FixedUpdate() //todo lo relacionado con fisicas siempre en fixed update
    {
        Movement();
        Jump();
    }
    private void LateUpdate() //el movimiento de la camara deberia ser aqui, no se porque no me pregunten pero se que es asi
    {
        Look();
    }
}
