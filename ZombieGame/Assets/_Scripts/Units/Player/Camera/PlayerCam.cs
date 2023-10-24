using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCam : MonoBehaviour
{
    [SerializeField] private float sensX;
    [SerializeField] private float sensY;
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform lookOrientation;

    float xRotation;
    float yRotation;

    private Vector2 look;

    public void OnLook(InputAction.CallbackContext ctx)
    {
        look = ctx.ReadValue<Vector2>();
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        float mouseX = look.x * Time.deltaTime * sensX;
        float mouseY = look.y * Time.deltaTime * sensY;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        lookOrientation.rotation = Quaternion.Euler(xRotation, yRotation, 0);

    }
}
