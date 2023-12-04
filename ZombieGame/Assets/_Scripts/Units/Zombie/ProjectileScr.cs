using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScr : MonoBehaviour
{
    [SerializeField] GameObject vomit;
    [SerializeField] int damage;
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);

        if (collision.gameObject.name=="Player")
        {
        }
        else
        {
            Vector3 pos = new Vector3(this.transform.position.x, this.transform.position.y-0.2f, this.transform.position.z);
            GameObject RB = Instantiate(vomit,pos,Quaternion.identity);
        }

        HealthComponent HC;
        if (collision.gameObject.TryGetComponent<HealthComponent>(out HC))
        {
            HC.TakeDamage(damage, new Vector3());
            Debug.Log("DAÑOOOO");
        }
    }
}
