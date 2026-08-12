using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectPrefabsHolder", menuName = "MyData/ProjectPrefabsHolder", order = 0)]
public class ProjectPrefabsHolder : SerializedScriptableObject
{
    [field: SerializeField] public AudioSourceObject AudioSourceObjectPrefab { get; private set; }
    [field: SerializeField] public SoundDataObject SoundDataObject { get; private set; }


}