using UnityEngine;

public class ProjectileScr : MonoBehaviour
{
    [SerializeField] GameObject vomit;
    [SerializeField] int damage;
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.name == "Player")
        {
        }
        else
        {
            Vector3 pos = new Vector3(this.transform.position.x, this.transform.position.y - 0.2f, this.transform.position.z);
            //GameObject RB = Instantiate(vomit, pos, Quaternion.identity);
            ObjectPoolingManager.Instance.SpawnObject(vomit, pos, Quaternion.identity, PoolType.GameObject);
        }

        HealthComponent HC;
        if (collision.gameObject.TryGetComponent<HealthComponent>(out HC))
        {
            HC.TakeDamage(damage, new Vector3());
        }
        //Destroy(this.gameObject);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        ObjectPoolingManager.Instance.ReturnObjectToPool(gameObject);
    }
}
