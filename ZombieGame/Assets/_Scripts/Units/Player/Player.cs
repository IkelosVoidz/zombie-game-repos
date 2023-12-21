using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    HealthComponent HC;
    private float _time = 0f;
    private float _healInterval = 1f;
    // Start is called before the first frame update
    void Start()
    {
        HC = GetComponent<HealthComponent>();
    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;

        if (_time >= _healInterval)
        {
            if (HC) HC.Heal(1);
            _time = 0f;
        }
    }
}
