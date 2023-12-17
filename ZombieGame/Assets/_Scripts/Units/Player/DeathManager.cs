using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathManager : MonoBehaviour
{
    public void YouLose()
    {
        PlayerInputManager.Instance.HandleUiInputSwitch(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +2);
    }
}
