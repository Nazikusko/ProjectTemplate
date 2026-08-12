using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class SceneObjectsHolder : MonoBehaviour
{
    [field: SerializeField] public Canvas UiCanvas { get; private set; }
    [field: SerializeField] public Camera Camera { get; private set; }
    [field: SerializeField] public ComponentPoolFactory DamageFlyInfoPool { get; private set; }
}
