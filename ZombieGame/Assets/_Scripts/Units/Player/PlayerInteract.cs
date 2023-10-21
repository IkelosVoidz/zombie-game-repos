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
        if (Physics.Raycast(_orientation.position, _orientation.forward, out RaycastHit hit, _interactionRange))
        {
            hit.transform.gameObject.TryGetComponent(out _interactable); //intentamos pillar el interactable de lo que sea que estamos mirando
            _interactable?.OnSelect();
        }
        else
        {
            _interactable?.OnDeselect();
            _interactable = null;
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
