using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class FadeAndDestroyMesh : MonoBehaviour
{
    private float _duration = 0f;
    private Material[] _materials;
    private void OnEnable()
    {
        _materials = gameObject.GetComponent<MeshRenderer>().materials;
    }

    public void StartFade(float delay , float fadeTime)
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
            foreach (var mat in _materials)
            {
                if (mat.HasProperty("_Color"))
                {
                    mat.color = new Color(
                        mat.color.r,
                        mat.color.g,
                        mat.color.b,
                        Mathf.Lerp(1, 0, t)
                        );
                }
            }
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}