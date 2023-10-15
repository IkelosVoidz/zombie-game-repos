using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private float _interactionRange;
    [SerializeField] private LayerMask _interactlayerMask;
    private IInteractable _interactable;
    [SerializeField] private Camera _cam;
  
    private void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out hit, _interactionRange, _interactlayerMask)){
            _interactable = hit.transform.gameObject.GetComponent<IInteractable>();
            Debug.Log("estas mirando un interactable");
        }
        else{
            _interactable=null;
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
