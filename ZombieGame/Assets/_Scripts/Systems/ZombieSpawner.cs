using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawn : MonoBehaviour
{
    [SerializeField] private GameObject zombiePrefab;
    [SerializeField] private Transform zombieSpawnPosition;
    [SerializeField] private GameObject player;
    private float repeatCycle = 1f;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.gameObject==player)
        {
            InvokeRepeating("EnemySpawner", 1f, repeatCycle);
            Destroy(gameObject, 10f);
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
        Debug.Log("nombre: "+ other.gameObject.tag);
    }

    void EnemySpawner()
    {
        Instantiate(zombiePrefab, zombieSpawnPosition.position, zombieSpawnPosition.rotation);

    }

}
