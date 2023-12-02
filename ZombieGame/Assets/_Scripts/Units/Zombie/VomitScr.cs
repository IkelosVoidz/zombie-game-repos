using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VomitScr : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(selfDestroy());
    }

    // Update is called once per frame


    IEnumerator selfDestroy()
    {
        Debug.Log("CCACACCAC");
        yield return new WaitForSeconds(5);
        ParticleSystem[] sistemasDeParticulas = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem sistema in sistemasDeParticulas)
        {
            sistema.Stop();
        }
    }
}
