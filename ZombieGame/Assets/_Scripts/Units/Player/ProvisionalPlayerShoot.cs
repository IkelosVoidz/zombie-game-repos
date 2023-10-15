using UnityEngine;
using UnityEngine.InputSystem;

public class ProvisionalPlayerShoot : MonoBehaviour
{
    [SerializeField] private Transform _orientation;
    [SerializeField] private LayerMask _shootLayerMask;
    public void OnFire(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (Physics.Raycast(_orientation.position, _orientation.forward, out RaycastHit hit, 1000f, _shootLayerMask))
            {
                HealthComponent attackedObjectHP;
                if (hit.transform.gameObject.TryGetComponent<HealthComponent>(out attackedObjectHP))
                {
                    attackedObjectHP.TakeDamage(1, _orientation.transform.forward); //this is very much a test
                    Debug.Log("hit on object with health");
                }
                else Debug.Log("hit on object without health");

            }
            else Debug.Log("miss");
        }
    }
}
