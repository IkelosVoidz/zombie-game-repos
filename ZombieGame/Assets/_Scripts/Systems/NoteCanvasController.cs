using TMPro;
using UnityEngine;

public class NoteCanvasController : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    private void Awake()
    {
        HideCanvas();
    }
    public void ShowNoteCanvas(ScriptableNote note)
    {
        PlayerInputManager.Instance.HandleUiInputSwitch(true);
        gameObject.SetActive(true);
        _text.SetText(note.noteContent);
        Time.timeScale = 0;
    }

    public void HideCanvas()
    {
        PlayerInputManager.Instance.HandleUiInputSwitch(false);
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}
