using System;
using Cinemachine;
using UnityEngine;
using Zenject;

public class CameraManager : IInitializable, IDisposable
{
    public event Action<bool> OnOffsetCameraStateChanged;
    public event Action<bool> OnCameraBlendComplete;
    public event Action<float> OnCameraBlendProgress;

    public Camera UiCamera { get; private set; }

    private CinemachineTransposer _cameraTansposer;
    private CinemachineVirtualCamera _mainCamera;
    private CinemachineVirtualCamera _offsetCamera;
    private CinemachineBrain _cinemachineBrain;

    [Inject] private Timer _timer;

    public bool IsOffsetCameraActive { get; private set; }

    private bool _isBlendStarted;

    public CameraManager(CameraManagerObjectsHolder objectsHolder)
    {
        _mainCamera = objectsHolder.MainCamera;
        _offsetCamera = objectsHolder.OffsetCamera;
        _cinemachineBrain = objectsHolder.Braine;
        UiCamera = objectsHolder.UiCamera;

        _cameraTansposer = _offsetCamera.GetCinemachineComponent<CinemachineTransposer>();
        //_cameraTansposer.m_FollowOffset += Vector3.forward * (-4f * CustomLerp(Screen.height / Screen.width));
    }

    public void Initialize()
    {
        _timer.OnTick += Update;
    }

    public void Dispose()
    {
        _timer.OnTick -= Update;
    }

    private void Update()
    {
        UpdateBlendEvents();
        UpdateBlendProgress();
    }

    private void UpdateBlendProgress()
    {
        if (!_cinemachineBrain.IsBlending || _cinemachineBrain.ActiveBlend == null) return;

        var blend = _cinemachineBrain.ActiveBlend;
        float t = blend.TimeInBlend / blend.Duration;
        float curvedT = blend.BlendCurve.Evaluate(t);
        OnCameraBlendProgress?.Invoke(curvedT);
    }

    private void UpdateBlendEvents()
    {
        if (_cinemachineBrain.IsBlending && !_isBlendStarted)
        {
            _isBlendStarted = true;
            OnOffsetCameraStateChanged?.Invoke(IsOffsetCameraActive);
            return;
        }

        if (!_cinemachineBrain.IsBlending && _isBlendStarted)
        {
            _isBlendStarted = false;
            OnCameraBlendComplete?.Invoke(IsOffsetCameraActive);
        }
    }

    public void RotateFirstCamera(float rotation)
    {
        _mainCamera.m_Lens.Dutch = rotation;
    }

    public void SetOffsetCamera(bool isOffsetCameraActive)
    {
        if (isOffsetCameraActive == IsOffsetCameraActive) return;

        IsOffsetCameraActive = isOffsetCameraActive;
        _mainCamera.Priority = isOffsetCameraActive ? 10 : 12;
        _offsetCamera.Priority = isOffsetCameraActive ? 12 : 10;
        SetCurvature(isOffsetCameraActive ? 0f : -0.00168f);
        RenderSettings.fog = !isOffsetCameraActive;
    }

    float CustomLerp(float x)
    {
        if (x <= 1.7f) return 1f;
        if (x >= 2.0f) return 0f;

        return Mathf.InverseLerp(2.0f, 1.7f, x);
    }

    public void SetCurvature(float curvature)
    {
        Shader.SetGlobalFloat("_Curvature", curvature);
    }
}
