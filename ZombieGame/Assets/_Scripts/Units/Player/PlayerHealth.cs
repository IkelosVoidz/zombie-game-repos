using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    HealthComponent HC;
    [SerializeField] private bool _godMode;
    private float _time = 0f;
    private float _heartSoundInterval = 1f;


    [SerializeField] private AudioClip[] _hurtSounds;
    [SerializeField] private AudioClip _heartSound;


    int _currentHealth;


    void Awake()
    {
        HC = GetComponent<HealthComponent>();
        _currentHealth = 100;
    }


    void Update()
    {
        if ((Input.GetKey(KeyCode.G)) && (Input.GetKey(KeyCode.M)))
        {
            HC._disabled = true;
        }


        _time += Time.deltaTime;

        if (_time >= _heartSoundInterval)
        {
            if (_currentHealth < 20) SoundManager.Instance.Play2DSoundFXClip(_heartSound, 0.8f);
            _time = 0f;
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


    public void OnPlayerHurt(int currentHealth, Vector3 unused2)
    {
        _currentHealth = currentHealth;
        SoundManager.Instance.Play2DRandomSoundFXClip(_hurtSounds, 0.8f);
    }
}

