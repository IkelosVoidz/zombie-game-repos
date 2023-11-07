using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private float _interactionRange;
    [SerializeField] private LayerMask _interactLayerMask;
    [SerializeField] private Transform _lookOrientation;
    private IInteractable _interactable;
    private IInteractable _newInteractable;


    private Ray _ray; //for debug purposes

    private void FixedUpdate()
    {
        _ray = new(_lookOrientation.position, _lookOrientation.forward);
        if (Physics.Raycast(_ray, out RaycastHit hit, _interactionRange, _interactLayerMask,QueryTriggerInteraction.Collide))
        {
            hit.transform.gameObject.TryGetComponent(out _newInteractable); //intentamos pillar el interactable de lo que sea que estamos mirando

            if (_newInteractable != _interactable) //si el interactable nuevo es diferente al anterior
            {
                _interactable?.OnDeselect(); //de seleccionamos el anterior
                _interactable = _newInteractable; //y seleccionamos el nuevo 
            }
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

    private void OnDrawGizmos() //for debug purposes
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(_ray);
    }
}
