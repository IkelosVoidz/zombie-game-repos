using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScr : MonoBehaviour
{
    [SerializeField] GameObject vomit;
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);

        if (collision.gameObject.name=="Player")
        {
            Debug.Log("Player");
        }
        else
        {
            Vector3 pos = new Vector3(this.transform.position.x, this.transform.position.y-0.2f, this.transform.position.z);
            GameObject RB = Instantiate(vomit,pos,Quaternion.identity);
            Debug.Log("Suelo");
        }
    }
}
