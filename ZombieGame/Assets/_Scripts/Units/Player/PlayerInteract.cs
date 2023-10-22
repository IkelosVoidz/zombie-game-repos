using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private float _interactionRange;
    [SerializeField] private LayerMask _interactLayerMask;
    [SerializeField] private Transform _orientation;
    private IInteractable _interactable;
    private IInteractable _newInteractable;


    private Ray _ray; //for debug purposes

    private void FixedUpdate()
    {
        _ray = new(_orientation.position, _orientation.forward);
        if (Physics.Raycast(_ray, out RaycastHit hit, _interactionRange, _interactLayerMask))
        {
            hit.transform.gameObject.TryGetComponent(out _newInteractable); //intentamos pillar el interactable de lo que sea que estamos mirando

            if (_newInteractable != _interactable)
            {
                _interactable?.OnDeselect();
                _interactable = _newInteractable;
            }
            _interactable?.OnSelect();
            Debug.Log("Estas mirando el interactuable: " + _interactable.gameObject.name);
        }
        else
        {
            Debug.Log("Ya no estas mirando el interactuable: " + _interactable?.gameObject.name);
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(_ray);
    }
}
