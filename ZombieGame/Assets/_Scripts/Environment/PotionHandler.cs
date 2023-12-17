using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PotionHandler : MonoBehaviour, IInteractable
{
    public UnityEvent onPotionMakerUsed;
    public UnityEvent onPotionMakerEnded;

    public void Interact()
    {
        onPotionMakerUsed?.Invoke();
    }

    public void OnTimerEnded()
    {
        PlayerInputManager.Instance.HandleUiInputSwitch(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
