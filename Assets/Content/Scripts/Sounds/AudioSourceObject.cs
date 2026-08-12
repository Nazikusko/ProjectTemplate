using UnityEngine;

public class AudioSourceObject : MonoBehaviour
{
    public float ClipLength => audioSource.clip.length;
    public SoundFxType SoundType { get; private set; }
    public bool Is3DSound { get; private set; }

    [SerializeField] private AudioSource audioSource;
    
    public AudioSource AudioSource => audioSource;

    public void Init(ClipDataHolder audioData, float volumeMultiplier, float pitch, bool is3dSound)
    {
        gameObject.SetActive(true);
        SoundType = audioData.SoundType;
        Is3DSound = is3dSound;
        audioSource.spatialBlend = is3dSound ? 1f : 0f;
        audioSource.clip = audioData.GetAudioClip();
        audioSource.volume = audioData.ClipVolume * volumeMultiplier;
        audioSource.pitch = pitch;
        audioSource.Play();
    }

    public void Disable()
    {
        audioSource.Stop();
        audioSource.volume = 0f;
        audioSource.clip = null;
        audioSource.pitch = 1;
        gameObject.SetActive(false);
    }
}
