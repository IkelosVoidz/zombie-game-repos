using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.InputSystem;

public class ProvisionalPlayerShoot : MonoBehaviour
{
    [SerializeField] private Camera _cam;
    [SerializeField] private LayerMask _shootLayerMask; 
    public void OnFire(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            RaycastHit hit;
            if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out hit, 1000f, _shootLayerMask))
            {
                HealthComponent attackedObjectHP;
                if (hit.transform.gameObject.TryGetComponent<HealthComponent>(out attackedObjectHP))
                {
                    attackedObjectHP.TakeDamage(1, _cam.transform.forward); //this is very much a test
                    Debug.Log("hit on object with health");
                }
                else Debug.Log("hit on object without health");

            }
            else Debug.Log("miss");
        }
    }
}
