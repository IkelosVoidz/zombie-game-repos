using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private float _interactionRange;
    [SerializeField] private LayerMask _interactLayerMask;
    [SerializeField] private Transform _orientation;
    private IInteractable _interactable;

    private void FixedUpdate()
    {
        if (Physics.Raycast(_orientation.position, _orientation.forward, out RaycastHit hit, _interactionRange, _interactLayerMask))
        {
            _interactable = hit.transform.gameObject.GetComponent<IInteractable>();
            Debug.Log("estas mirando un interactable");
        }
        else
        {
            _interactable = null;
            Debug.Log("ya no estas mirando un interactable");
        }
    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            _interactable?.Interact();
            Debug.Log("estas interactuando");
        }
    }
}
