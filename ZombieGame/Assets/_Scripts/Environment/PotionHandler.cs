using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PotionHandler : MonoBehaviour, IInteractable
{
    [SerializeField] AudioClip startPotionMaker;

    public UnityEvent onPotionMakerUsed;
    public UnityEvent onPotionMakerEnded;

    public void Interact()
    {
        SoundManager.Instance.PlaySoundFXClip(startPotionMaker, transform, .7f);
        onPotionMakerUsed?.Invoke();
    }

    public void OnTimerEnded()
    {
        PlayerInputManager.Instance.HandleUiInputSwitch(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
