using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;


[CreateAssetMenu(fileName = "SoundData", menuName = "MyData/SoundData", order = 5)]

public class SoundDataObject : ScriptableObject
{
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private List<ClipDataHolder> _clipsData = new List<ClipDataHolder>();

    public List<ClipDataHolder> ClipsDataList => _clipsData;
    public AudioMixer AudioMixer => _audioMixer;
}

[Serializable]
public class ClipDataHolder
{
    [SerializeField] private SoundFxType _soundType;
    [SerializeField] private bool _isDisabled = false;
    [SerializeField] private AudioClip[] _clips;
    [Range(0, 1f)]
    [SerializeField] private float _clipVolume = 1f;

    public SoundFxType SoundType => _soundType;
    public float ClipVolume => _clipVolume;
    public bool IsDisabled => _isDisabled;

    public AudioClip GetAudioClip()
    {
        return _clips[Random.Range(0, _clips.Length)];
    }
}

public enum SoundFxType
{
    None = -1,
    MainMenu,
    LobbyMusic,
    MusicGamePlay1,

    TapButton = 20,
}