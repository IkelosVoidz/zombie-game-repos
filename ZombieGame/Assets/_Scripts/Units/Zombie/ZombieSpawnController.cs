using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class ZombieSpawnController : MonoBehaviour
{
    [SerializeField, Tooltip("")] private GameObject _zombiePrefab;
    [SerializeField, Tooltip("")] private GameObject _zombiePrefab2;
    [SerializeField, Tooltip("")] private GameObject _zombiePrefab3;

    [SerializeField, Tooltip("")] private GameObject _target;

    [SerializeField, Tooltip("")] private int _initialZombiesPerWave;
    private int _currentZombiesPerWave;


    [SerializeField, Tooltip("")] private float _spawnDelay; //Delay entre zombie y zombie spawneado


    [SerializeField, Tooltip("")] private int _currentWave;
    [SerializeField, Tooltip("")] private float _waveCoolDown;

    private bool inCooldown;

    [SerializeField, Tooltip("")] private float _coolDownCounter;


    private List<Zombie> _currentZombiesAlive;
    private float _multiplier;



    private void Start()
    {
        _currentZombiesAlive = new List<Zombie>();
        _currentZombiesPerWave = _initialZombiesPerWave;
        _multiplier = 1.09f;
        StartNextWave();
    }

    private void StartNextWave()
    {
        _currentZombiesAlive.Clear();
        _currentWave++;
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        for (int i = 0; i < _currentZombiesPerWave; i++)
        {
            Vector3 spawnOffset = new Vector3(Random.Range(-1f,1f),0f,Random.Range(-1f,1f));
            Vector3 spawnPosition = transform.position + spawnOffset;

            GameObject selectedZombiePrefab = GetRandomZombiePrefab();

            var zombie = Instantiate(selectedZombiePrefab, spawnPosition, Quaternion.identity);

            Zombie zombieScript = zombie.GetComponent<Zombie>();
            zombieScript.target = _target; //ASIGNO EL TARGET QUE TIENE EL ZOMBIE

            _currentZombiesAlive.Add(zombieScript);


            yield return new WaitForSeconds(_spawnDelay);
        }
    }

    private void Update()
    {
        List<Zombie> zombiesToRemove = new List<Zombie>();
        foreach (Zombie zomb in _currentZombiesAlive)
        {
            if(zomb.isDead()){
                zombiesToRemove.Add(zomb);
            }
        }

        foreach (Zombie zombie in zombiesToRemove)
        {
            _currentZombiesAlive.Remove(zombie);
        }

        zombiesToRemove.Clear();

        //Cooldown si todos los zombies estan muertos

        if(_currentZombiesAlive.Count==0 && inCooldown == false)
        {
            StartCoroutine(WaveCoolDown());
        }

        //Run the cooldown counter

        if (inCooldown)
            _coolDownCounter -= Time.deltaTime;

        else
            _coolDownCounter = _waveCoolDown;

    }

    private GameObject GetRandomZombiePrefab()
    {
        List<GameObject> zombiePrefabs = new List<GameObject> { _zombiePrefab, _zombiePrefab2, _zombiePrefab3 };
        int randomIndex = Random.Range(0, zombiePrefabs.Count);
        return zombiePrefabs[randomIndex];
    }

    private IEnumerator WaveCoolDown()
    {
        inCooldown = true;
        yield return new WaitForSeconds(_waveCoolDown);
        inCooldown = false;

        _currentZombiesPerWave = Mathf.CeilToInt(_currentZombiesPerWave * _multiplier);
        StartNextWave();
    }

    public void ChangeSpawn() {
        Debug.Log("SPAWN MODIFICADO");
        _multiplier = 1.15f;
    }
}
