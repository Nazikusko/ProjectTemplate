using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputManager : IDisposable
{
    public event Action<Vector2> OnInputDown;
    public event Action<Vector2, Vector2, Vector2> OnDrag;
    public event Action<Vector2, Vector2, Vector2> OnInputUp;

    public bool IsSwiping { get; private set; }

    private Timer _timer;
    public GraphicRaycaster _uiRaycaster;
    private Vector2 _startTouchPosition;
    private Vector2 _currentTouchPosition;
    private Vector2 _prevFrameTouchPosition;

    public void Init(GraphicRaycaster uiRaycaster, Timer timer)
    {
        IsSwiping = false;
        _timer = timer;
        _uiRaycaster = uiRaycaster;
        _startTouchPosition = Vector2.zero;
        _currentTouchPosition = Vector2.zero;
        _prevFrameTouchPosition = Vector2.zero;
        _timer.OnTick += Update;
    }

    public void Dispose()
    {
        OnInputDown = null;
        OnDrag = null;
        OnInputUp = null;

        IsSwiping = false;
        _timer.OnTick -= Update;
        _timer = null;
        _uiRaycaster = null;
    }

    void Update()
    {
        if (Application.isEditor)
        {
            UpdateEditor();
        }
        else
        {
            UpdateDevice();
        }
    }

    public bool TryGetComponentOnUi<T>(Vector2 screenPosition, out T resultComponent, bool raycastOnlyFirstObject = false) where T : Component
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = screenPosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        //_uiRaycaster.Raycast(pointerData, results);
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.tag == "TutorMask")
            {
                foreach (var newResult in results)
                {
                    bool isHaveCanvas = newResult.gameObject.TryGetComponent<Canvas>(out var canvas);
                    bool isHaveT = newResult.gameObject.TryGetComponent<T>(out resultComponent);
                    if (isHaveCanvas && isHaveT)
                    {
                        return true;
                    }
                }

                resultComponent = null;
                return false;
            }

            if (result.gameObject.TryGetComponent<T>(out resultComponent))
            {
                return true;
            }

            if (raycastOnlyFirstObject) break;
        }

        resultComponent = null;
        return false;
    }

    public bool IsPointerOverUIObject(Vector2 pointerPosition)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = pointerPosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        _uiRaycaster.Raycast(eventData, results);
        return results.Count == 0;
    }

    private void UpdateEditor()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _startTouchPosition = Input.mousePosition;
            _prevFrameTouchPosition = _startTouchPosition;
            OnInputDown?.Invoke(_startTouchPosition);
        }

        if (Input.GetMouseButton(0))
        {
            IsSwiping = true;
            _currentTouchPosition = Input.mousePosition;
            OnDrag?.Invoke(_currentTouchPosition, _currentTouchPosition - _prevFrameTouchPosition, _currentTouchPosition - _startTouchPosition);
            _prevFrameTouchPosition = _currentTouchPosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            IsSwiping = false;
            _currentTouchPosition = Input.mousePosition;
            OnInputUp?.Invoke(_currentTouchPosition, _currentTouchPosition - _prevFrameTouchPosition, _currentTouchPosition - _startTouchPosition);
            _prevFrameTouchPosition = Vector2.zero;
            _startTouchPosition = Vector2.zero;
        }
    }

    private void UpdateDevice()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _startTouchPosition = touch.position;
                    OnInputDown?.Invoke(touch.position);
                    break;

                case TouchPhase.Moved:
                    IsSwiping = true;
                    _currentTouchPosition = touch.position;
                    OnDrag?.Invoke(_currentTouchPosition, touch.deltaPosition, _currentTouchPosition - _startTouchPosition);
                    break;

                case TouchPhase.Ended or TouchPhase.Canceled:
                    IsSwiping = false;
                    _currentTouchPosition = touch.position;
                    OnInputUp?.Invoke(_currentTouchPosition, touch.deltaPosition, _currentTouchPosition - _startTouchPosition);
                    _startTouchPosition = Vector2.zero;
                    break;
            }
        }
    }
}
