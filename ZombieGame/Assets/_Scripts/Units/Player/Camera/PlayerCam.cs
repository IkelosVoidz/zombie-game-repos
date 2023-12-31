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

    [HideInInspector]
    public Vector2 look { get; private set; }

    public void OnLook(InputAction.CallbackContext ctx)
    {
        look = ctx.ReadValue<Vector2>();
    }
    private void Start()
    {
        //FALTA PULIR-HO
        ChangeSensi();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SettingsManager.Instance.changed = true;
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
    public void ChangeSensi()
    {
        if (SettingsManager.Instance.changed)
        {
            sensX = SettingsManager.Instance.sensi;
            sensY = SettingsManager.Instance.sensi;
        }
    }
}
