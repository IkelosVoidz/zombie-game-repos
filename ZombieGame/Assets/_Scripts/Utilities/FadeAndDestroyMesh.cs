using System.Collections;
using UnityEngine;
public class FadeAndDestroyMesh : MonoBehaviour
{
    private float _duration = 0f;
    private Material[] _materials;
    [SerializeField] public FadeType _type;
    private void OnEnable()
    {
        Renderer renderer = GetComponent<Renderer>();

        if (renderer != null)
            _materials = renderer.materials;
        else
        {
            renderer = GetComponentInChildren<SkinnedMeshRenderer>();
            _materials = renderer.materials;
        }
    }

    public void StartFade(float delay, float fadeTime)
    {
        _duration = fadeTime;
        Invoke("InvokeCorroutine", delay);
    }
    public void InvokeCorroutine()
    {
        StartCoroutine(FadeOutAndDestroy());
    }
    IEnumerator FadeOutAndDestroy()
    {
        float timeElapsed = 0;

        while (timeElapsed < _duration)
        {
            float t = timeElapsed / _duration;
            switch (_type)
            {
                case FadeType.FADE:
                    for (int i = 0; i < _materials.Length; i++)
                    {
                        JJC1138_RuntimeFadeFix(ref _materials[i]);

                        if (_materials[i].HasProperty("_Color"))
                        {
                            var color = new Color(
                                _materials[i].color.r,
                                _materials[i].color.g,
                                _materials[i].color.b,
                                Mathf.Lerp(1, 0, t)
                                );
                            _materials[i].SetColor("_Color", color);
                        }
                    }
                    break;
                case FadeType.DWINDLE_SIZE:
                    transform.localScale = new Vector3(Mathf.Lerp(1, 0, t), Mathf.Lerp(1, 0, t), Mathf.Lerp(1, 0, t));
                    break;
                default:
                    break;
            }

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }


    //Big thanks to JJC1138 on the unity forums for this fix (dated Feb 12nd 2015)
    //I was unable to change the alpha value of the material before due to having to change the rendering mode
    //on the material on the project view, and that changed how other objects in the scene looked
    //By doing it like this i can change the rendering mode at runtime, and on top of that the material becomes an instance of the original material
    //so it doesnt modify the other objects which have the same material.
    //I was stuck on this tiny problem for days, so i couldnt have done it without him.
    /// <summary>
    /// Will modify the instance of the material to switch it to rendering mode "fade", so that it can be faded out
    /// <param name="mat">reference to the Material to update</param>
    /// </summary>
    void JJC1138_RuntimeFadeFix(ref Material mat)
    {
        mat.SetFloat("_Mode", 2);
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.EnableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = 3000;
    }
}

public enum FadeType
{
    FADE, DWINDLE_SIZE
}