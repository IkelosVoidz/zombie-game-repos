using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Slider _sensitivity;

    [SerializeField] private GameObject _options;
    [SerializeField] private GameObject _menu;

    public void Start()
    {
        _options.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ResumeGame();
        }
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void ResumeGame()
    {
        _options.SetActive(false);
        _menu.SetActive(true);
    }

    public void SensitivityChanged()
    {
        //Debug.Log(_sensitivity.value);
        SettingsManager.Instance.sensi=_sensitivity.value;
        SettingsManager.Instance.changed = true;
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
