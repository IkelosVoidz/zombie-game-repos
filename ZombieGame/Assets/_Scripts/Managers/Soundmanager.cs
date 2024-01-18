using UnityEngine;
using UnityEngine.Audio;
public class SoundManager : PersistentSingleton<SoundManager>
{
    [SerializeField] private AudioSource _musicSource, _effectsSource;
    [SerializeField] private AudioMixer _audioMixer;

    //provisional, esto seran los sliders en el menu de opciones
    [SerializeField, Range(0.0001f, 1)] private float _masterVolume;
    [SerializeField, Range(0.0001f, 1)] private float _soundFXVolume;
    [SerializeField, Range(0.0001f, 1)] private float _MusicVolume;
    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        //spawn in audiosource
        GameObject g = ObjectPoolingManager.Instance.SpawnObject(_effectsSource.gameObject, spawnTransform.position, spawnTransform.rotation, PoolType.AudioSources);
        AudioSource audioSource = g.GetComponent<AudioSource>();

        //asign audio clip
        audioSource.clip = audioClip;
        //assign volume
        audioSource.volume = volume;
        //play sound
        audioSource.Play();

        //get length of sound FX clip
        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }

    public void PlayRandomSoundFXClip(AudioClip[] audioClip, Transform spawnTransform, float volume)
    {
        int rand = Random.Range(0, audioClip.Length);

        //spawn in audiosource
        GameObject g = ObjectPoolingManager.Instance.SpawnObject(_effectsSource.gameObject, spawnTransform.position, spawnTransform.rotation, PoolType.AudioSources);
        AudioSource audioSource = g.GetComponent<AudioSource>();

        //asign audio clip
        audioSource.clip = audioClip[rand];
        //assign volume
        audioSource.volume = volume;
        //play sound
        audioSource.Play();
        //get length of sound FX clip
        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }


    //ULTRA MEGA PROVISIONAL DIOS NO ME PEGUEN
    private void Update()
    {
        SetMasterVolume(_masterVolume);
        SetSoundFXVolume(_soundFXVolume);
        SetMusicVolume(_MusicVolume);
    }

    public void SetMasterVolume(float level)
    {
        _audioMixer.SetFloat("MasterVolume", Mathf.Log10(level) * 20);
        //magia matematica para hacer que sea lineal y no logaritmico
    }

    public void SetSoundFXVolume(float level)
    {
        _audioMixer.SetFloat("SoundFXVolume", Mathf.Log10(level) * 20);
    }

    public void SetMusicVolume(float level)
    {
        _audioMixer.SetFloat("MusicVolume", Mathf.Log10(level) * 20);
    }

}
