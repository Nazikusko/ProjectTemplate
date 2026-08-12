using Cinemachine;
using UnityEngine;

public class CameraManagerObjectsHolder : MonoBehaviour
{
    [field: SerializeField] public CinemachineVirtualCamera MainCamera { get; private set; }
    [field: SerializeField] public Camera UiCamera { get; private set; }
    [field: SerializeField] public CinemachineVirtualCamera OffsetCamera { get; private set; }
    [field: SerializeField] public CinemachineBrain Braine { get; private set; }
}
