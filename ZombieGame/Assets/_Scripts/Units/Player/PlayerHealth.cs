using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    HealthComponent HC;
    [SerializeField] private bool _godMode;
    /*private float _time = 0f;
    private float _healInterval = 1f;*/
    void Awake()
    {
        HC = GetComponent<HealthComponent>();
    }

    /*void Update()
    {
        _time += Time.deltaTime;

        if (_time >= _healInterval)
        {
            if (HC) HC.Heal(1);
            _time = 0f;
        }
    }*/


    private void Update()
    {
        if ((Input.GetKey(KeyCode.G)) && (Input.GetKey(KeyCode.M)))
        {
            HC._disabled = true;
        }
    }

    private void OnEnable()
    {
        PickableHealth.OnHealthPickup += HealthPicked;
    }

    private void OnDisable()
    {
        PickableHealth.OnHealthPickup -= HealthPicked;
    }

    void HealthPicked()
    {
        HC.Heal(20);
    }
}
