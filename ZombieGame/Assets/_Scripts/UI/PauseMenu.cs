using System;
using UnityEngine;
using UnityEngine.UI;
public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;

    [SerializeField] private GameObject _options;

    [SerializeField] private Slider _sensitivity;
    [SerializeField] private Slider _masterSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _soundFXSlider;
    [SerializeField] private Slider _ambienceSlider;


    private Camera _mainCamera;


    public static event Action OnSensitivityChanged;

    public bool isPaused;
    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        _options.SetActive(false);
        _sensitivity.value = SettingsManager.Instance.sensi;
        OnSensitivityChanged?.Invoke();

        SoundManager.Instance.InitializeVolume(_options); //no tocar
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    public void PauseGame()
    {
        PlayerInputManager.Instance.HandleUiInputSwitch(true);
        pauseMenu.SetActive(true);
        _options.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        PlayerInputManager.Instance.HandleUiInputSwitch(false);
        pauseMenu.SetActive(false);
        _options.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SensitivityChanged()
    {
        SettingsManager.Instance.sensi = _sensitivity.value;
        OnSensitivityChanged?.Invoke();
    }

    private void OnEnable()
    {
        _sensitivity.onValueChanged.AddListener(delegate { SensitivityChanged(); });
        _masterSlider.onValueChanged.AddListener(delegate { SensitivityChanged(); });
        _musicSlider.onValueChanged.AddListener(delegate { SensitivityChanged(); });
        _soundFXSlider.onValueChanged.AddListener(delegate { SensitivityChanged(); });
        _ambienceSlider.onValueChanged.AddListener(delegate { SensitivityChanged(); });
    }

    private void OnDisable()
    {
        _sensitivity.onValueChanged.RemoveAllListeners();

    }



}
