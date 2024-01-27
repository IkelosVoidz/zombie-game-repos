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
        GunAnimEvents.OnChamberAnim += OnReloadChamber;
        GunAnimEvents.OnMagDropAnim += OnReloadMagDropped;
        GunAnimEvents.OnMagInsertAnim += OnReloadMagInsert;
    }


    private void OnDisable()
    {
        GunAnimEvents.OnChamberAnim -= OnReloadChamber;
        GunAnimEvents.OnMagDropAnim -= OnReloadMagDropped;
        GunAnimEvents.OnMagInsertAnim -= OnReloadMagInsert;
    }


    void OnReloadMagDropped()
    {
        SoundManager.Instance.Play2DSoundFXClip(_reloadMagDrop, 1f);
    }

    void OnReloadMagInsert()
    {
        SoundManager.Instance.Play2DSoundFXClip(_reloadMagInsert, 1f);
    }

    void OnReloadChamber()
    {
        SoundManager.Instance.Play2DSoundFXClip(_reloadGunChamber, 1f);
    }

}
