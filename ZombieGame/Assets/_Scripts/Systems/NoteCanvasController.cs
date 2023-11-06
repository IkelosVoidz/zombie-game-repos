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
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        gameObject.SetActive(true);
        _text.SetText(note.noteContent);
        Time.timeScale = 0;
    }

    public void HideCanvas()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}
