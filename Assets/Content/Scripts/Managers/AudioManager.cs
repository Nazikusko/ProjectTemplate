using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class AudioManager : IInitializable, IDisposable
{
    public bool IsMusicPlaying => _musicSource.isPlaying;

    private const float AUIDIO_ON_VOLUME_VALUE = 0;
    private const float AUIDIO_OFF_VOLUME_VALUE = -80f;
    private const float AUIDIO_NON_LISTEN_VOLUME_VALUE = -55f;

    private AudioSource _musicSource;
    private SoundDataObject _soundDataObject;
    private ComponentPoolFactory _audioPool;
    private ComponentPoolFactory _placeIndependentAudioPool;
    private List<AudioSourceObject> _activeAudioSources;

    [Inject] private SettingsSaveModel _gameSettings;
    [Inject] private DiContainer _diContainer;
    [Inject] private ProjectPrefabsHolder _prefabsHolder;
    [Inject] private SoundSourcesHolder _soundSourcesHolder;

    public void Initialize()
    {
        _musicSource = _soundSourcesHolder.MusicAudioSource;
        _soundDataObject = _prefabsHolder.SoundDataObject;
        _placeIndependentAudioPool = _soundSourcesHolder.PlaceIndependentAudioPool;
        _audioPool = _soundSourcesHolder.SoundEffectAudioSourcePool;
        _activeAudioSources = new List<AudioSourceObject>();
        ChangeVolumeEffects(_gameSettings.SfxVolume);
        ChangeVolumeMusic(_gameSettings.MusicVolume);
    }

    public void Dispose()
    {

    }

    public void PlaySound(SoundFxType soundFxType, Vector3 position, float volumeMultiplier = 1f, float pitch = 1f)
    {
        if (!_gameSettings.IsSfxOn) return;

        ClipDataHolder curClip = _soundDataObject.ClipsDataList.Find(x => x.SoundType == soundFxType);
        if (curClip == null || curClip.IsDisabled) return;

        _soundSourcesHolder.StartCoroutine(AudioPlayCoroutine(curClip, false, position, volumeMultiplier, pitch));
    }

    public void PlaySoundPositionIndependent(SoundFxType soundFxType, float volumeMultiplier = 1f, float pitch = 1f)
    {
        if (!_gameSettings.IsSfxOn) return;

        ClipDataHolder curClip = _soundDataObject.ClipsDataList.Find(x => x.SoundType == soundFxType);
        if (curClip == null || curClip.IsDisabled) return;

        _soundSourcesHolder.StartCoroutine(AudioPlayCoroutine(curClip, true, Vector3.zero, volumeMultiplier, pitch));
    }

    public void StopPlaySound(SoundFxType soundFxType)
    {
        if (!_gameSettings.IsSfxOn) return;

        var audioSourceObjects = _activeAudioSources.FindAll(x => x.SoundType == soundFxType);
        foreach (var audioSourceObject in audioSourceObjects)
        {
            audioSourceObject.Disable();
            if (audioSourceObject.Is3DSound)
            {
                _audioPool.Release(audioSourceObject);
            }
            else
            {
                _placeIndependentAudioPool.Release(audioSourceObject);
            }

            _activeAudioSources.Remove(audioSourceObject);
        }
    }

    public bool IsSoundPlaying(SoundFxType soundFxType)
    {
        return _activeAudioSources.Exists(x => x.SoundType == soundFxType);
    }

    private IEnumerator AudioPlayCoroutine(ClipDataHolder audioData, bool isPlaceIndependent, Vector3 position, float volumeMultiplier, float pitch)
    {
        AudioSourceObject audioSourceObject = isPlaceIndependent
            ? _placeIndependentAudioPool.Get<AudioSourceObject>()
            : _audioPool.Get<AudioSourceObject>();

        audioSourceObject.Init(audioData, volumeMultiplier, pitch, !isPlaceIndependent);
        _activeAudioSources.Add(audioSourceObject);

        if (!isPlaceIndependent)
        {
            audioSourceObject.transform.position = position;
        }

        yield return new WaitForSeconds(audioSourceObject.ClipLength);

        audioSourceObject.Disable();
        _activeAudioSources.Remove(audioSourceObject);

        if (isPlaceIndependent)
        {
            _placeIndependentAudioPool.Release(audioSourceObject);
        }
        else
        {
            _audioPool.Release(audioSourceObject);
        }
    }

    public void PlayMusic(SoundFxType musicType)
    {
        if (!_gameSettings.IsMusicOn) return;

        ClipDataHolder curClip = _soundDataObject.ClipsDataList.Find(x => x.SoundType == musicType);
        if (curClip == null) return;

        _musicSource.clip = curClip.GetAudioClip();
        _musicSource.volume = curClip.ClipVolume;
        _musicSource.Play();
    }

    public void StopPlayMusic()
    {
        if (_musicSource.isPlaying)
            _musicSource.Stop();
    }

    #region VolumeSettings

    public void ToggleVolumeEffects(bool enabled)
    {
        _soundDataObject.AudioMixer.SetFloat("EffectVolume", enabled ? AUIDIO_ON_VOLUME_VALUE : AUIDIO_OFF_VOLUME_VALUE);
    }

    public void ToggleVolumeMusic(bool enabled)
    {
        _soundDataObject.AudioMixer.SetFloat("MusicVolume", enabled ? AUIDIO_ON_VOLUME_VALUE : AUIDIO_OFF_VOLUME_VALUE);
    }

    public void ChangeVolumeEffects(float volume)
    {
        if (volume > 0.01f)
        {
            _soundDataObject.AudioMixer.SetFloat("EffectVolume",
                Mathf.Lerp(AUIDIO_NON_LISTEN_VOLUME_VALUE, AUIDIO_ON_VOLUME_VALUE, volume));
        }
        else
        {
            ToggleVolumeEffects(false);
        }
    }

    public void ChangeVolumeMusic(float volume)
    {
        if (volume > 0.01f)
        {
            _soundDataObject.AudioMixer.SetFloat("MusicVolume",
                Mathf.Lerp(AUIDIO_NON_LISTEN_VOLUME_VALUE, AUIDIO_ON_VOLUME_VALUE, volume));
        }
        else
        {
            ToggleVolumeMusic(false);
        }
    }

    #endregion
}