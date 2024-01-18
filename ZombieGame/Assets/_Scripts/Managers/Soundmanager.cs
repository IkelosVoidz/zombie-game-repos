using UnityEngine;

public class SoundManager : PersistentSingleton<SoundManager>
{
    [SerializeField] private AudioSource _musicSource, _effectsSource;
    /*// Start is called before the first frame update
    
    public void PlaySound(AudioClip clip)
    {
        _effectsSource.PlayOneShot(clip);
    }

    //hay que implementar esto de forma dinamica, es decir, que se vayan creando AudioSources en posicion
   //porque si no solo se puede reproducir un audio a la vez
    public void PlaySoundAtPoint(AudioClip clip, Vector3 point) 
    {

    }

    public void ChangeMasterVolume(float value) //que se llame desde un menu de opciones o algo asi 
    {
        AudioListener.volume = value;
    }*/

    public void PlaySoundFXClipAtPoint(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        //spawn in audiosource
        AudioSource audioSource = Instantiate(_effectsSource, spawnTransform);
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

}
