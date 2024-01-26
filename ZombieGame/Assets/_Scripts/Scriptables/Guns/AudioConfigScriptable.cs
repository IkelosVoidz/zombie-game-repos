using UnityEngine;

[CreateAssetMenu(fileName = "AudioCfg", menuName = "Weapons/AudioCfg", order = 0)]
public class AudioConfigScriptable : ScriptableObject
{
    [SerializeField] public AudioClip[] _fireSounds;
    [SerializeField] public AudioClip _emptyClip;
    [SerializeField] public AudioClip _reloadMagDrop;
    [SerializeField] public AudioClip _reloadMagInsert;
    [SerializeField] public AudioClip _reloadGunChamber;


    private void OnEnable()
    {

    }


    private void OnDisable()
    {

    }


    void OnReloadMagDropped()
    {

    }

    void OnReloadMagInsert()
    {

    }

    void OnReloadChamber()
    {

    }

}
