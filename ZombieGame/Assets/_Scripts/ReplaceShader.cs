using UnityEngine;

public class ReplaceShader : MonoBehaviour
{
    [SerializeField] Shader _replacement;
    [SerializeField] Camera _camera;



    private void OnEnable()
    {
        _camera.SetReplacementShader(_replacement, "RenderType");
    }

    private void OnDisable()
    {
        _camera.ResetReplacementShader();
    }
}
