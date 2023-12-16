using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VomitScr : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] int damage = 1;
    private bool canDamage = true;
    private float time = 0;

    void Start()
    {
        StartCoroutine(selfDestroy());
    }

    // Update is called once per frame

    private void Update()
    {
        if (!canDamage)
        {
            time +=Time.deltaTime;
            if(time > 1f)
            {
                canDamage = true;
                time = 0;
            }
        }
    }


    IEnumerator selfDestroy()
    {
        yield return new WaitForSeconds(5);
        ParticleSystem[] sistemasDeParticulas = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem sistema in sistemasDeParticulas)
        {
            sistema.Stop();
        }
        destroyObj();
    }

    IEnumerator destroyObj()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
    private void OnTriggerStay(Collider other)
    {
        if (canDamage)
        {
            canDamage = false;
            HealthComponent HC = other.GetComponentInParent<HealthComponent>();
            Debug.Log(other.name);

            if (HC is not null && other.name=="PlayerObj")
            {
                Debug.Log("Daño al jugador");
                HC.TakeDamage(damage, new Vector3());
            }

            //if(other.gameObject.TryGetComponent<HealthComponent>(out HC))
            //{

            //}
        }
        
    }
}
