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
    public GameObject Camera;
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
        //Creamos el vector de Velocidad del personage
        Vector3 currentVelocity = rb.velocity;
        Vector3 targetVelocity = new Vector3(move.x,0,move.y);
        targetVelocity *= speed; //Capo la velocidad del personage al valor de "speed"


        //Alinear Direccion
        targetVelocity = transform.TransformDirection(targetVelocity); //Sin esto el Player solo se mueve WASD en un mismo eje. No acompaña al movimiento de la camara.


        //Calcular fuerzas
        Vector3 velocityChange = (targetVelocity - currentVelocity);
        velocityChange = new Vector3 (velocityChange.x, 0, velocityChange.z); //Delimito la "velocityChange.y" a 0 para que el "player" caiga. Si no es igual a 0 el player simplemente se comoporta como si no tubiese gravedad.

        //Limitar Fuerza (Entiendo que maxForce siempre es igual a 1)
        Vector3.ClampMagnitude(velocityChange,maxForce);


        //Aplicar movimiento al RB
        rb.AddForce(velocityChange,ForceMode.VelocityChange);

    }
    public void Look()
    {
   
        transform.Rotate(Vector3.up * look.x * sensitivity); //La camara se mueve acorde a la sensivilidad.
        lookRotation += (-look.y * sensitivity);
        lookRotation = Mathf.Clamp(lookRotation,-90,90); //Delimito la camara para que se mueva 90 grados arriba y abajo
        Camera.transform.eulerAngles = new Vector3(lookRotation,Camera.transform.eulerAngles.y, Camera.transform.eulerAngles.z); //Asignar rotaciones

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
