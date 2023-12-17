using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Slider _sensitivity;

    [SerializeField] private GameObject _options;

    public void Start()
    {
        _options.SetActive(false);
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void SensitivityChanged()
    {
        //Debug.Log(_sensitivity.value);
        SettingsManager.Instance.sensi=_sensitivity.value;
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
