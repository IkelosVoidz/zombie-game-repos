using UnityEngine;
using UnityEngine.UI;
public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;

    [SerializeField] private GameObject _options;

    [SerializeField] private Slider _sensitivity;

    public bool isPaused;
    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        _options.SetActive(false);
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
    }

    private void OnEnable()
    {
        _sensitivity.onValueChanged.AddListener(delegate { SensitivityChanged(); });
    }

    private void OnDisable()
    {
        _sensitivity.onValueChanged.RemoveAllListeners();
    }
    
}
