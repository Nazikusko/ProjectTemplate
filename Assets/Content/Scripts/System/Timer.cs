using System;
using UnityEngine;

public class Timer
{
    public event Action OnTick;
    public event Action OnPostTick;
    public event Action OnFixedTick;
    public event Action OnSecondTick;

    private float _oneSecondTimer;

    public void Update()
    {
        _oneSecondTimer += Time.deltaTime;
        OnTick?.Invoke();

        if (_oneSecondTimer >= 1)
        {
            _oneSecondTimer = 0;
            OnSecondTick?.Invoke();
        }
    }

    public void LateUpdate()
    {
        OnPostTick?.Invoke();
    }

    public void FixedUpdate()
    {
        OnFixedTick?.Invoke();
    }
}