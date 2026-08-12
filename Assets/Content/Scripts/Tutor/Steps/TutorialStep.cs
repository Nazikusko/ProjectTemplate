using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

public abstract class TutorialStep : IDisposable
{
    public const string TUTORIAL_SKINS_PATH = "Tutor/Skins/{0}";
    public const string SKIP_BUTTON_PATH = "Tutor/Objects/";

    public Action OnComplete;

    public bool IsShown => _goTutorialHint != null;

    [Inject] protected TutorialManager _tutorialManager;
    [Inject] protected UILinkManager _uiLinkManager;

    private Image _blockraycastImage;
    private GameObject _skipButtonObject;
    private Button _skipButton;

    protected GameObject _goTutorialHint;

    public abstract void Initialize();
    public abstract void Dispose();

    protected void CompleteThisStep()
    {
        RemoveSkipTutorialButton();
        OnComplete?.Invoke();
    }

    protected void RemoveSkipTutorialButton()
    {
        if (_skipButton != null)
        {
            GameObject.Destroy(_skipButtonObject);
        }
    }

    private void ShowSkipButton(Transform buttonTransform)
    {
        _skipButtonObject = GameObject.Instantiate(Resources.Load<GameObject>(SKIP_BUTTON_PATH + "SkipTutorButton"), buttonTransform);
        _skipButton = _skipButtonObject.GetComponentInChildren<Button>();
        _skipButton.onClick.AddListener(RemoveSkipTutorialButton);
        _skipButton.onClick.AddListener(_tutorialManager.SkipTutorial);
        Canvas tmpCanvas = _skipButtonObject.AddComponent<Canvas>();
        tmpCanvas.overrideSorting = true;
        tmpCanvas.sortingLayerName = "UI";
        tmpCanvas.sortingOrder = 4;
        _skipButtonObject.AddComponent<AlwaysRaycastMask>();
        _skipButtonObject.AddComponent<GraphicRaycaster>();
    }

    protected GameObject ShowTutorialHint(Component target)
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        ShowSkipButton(_tutorialManager.TutorialUiLayerTransform);
#endif
        var path = string.Format(TUTORIAL_SKINS_PATH, this.GetType().Name);
        _goTutorialHint = GameObject.Instantiate(Resources.Load<GameObject>(path), _tutorialManager.TutorialUiLayerTransform);
        _goTutorialHint.GetComponentInChildren<TutorHand>().PointedHandCanvasObject(target.transform);
        return _goTutorialHint;
    }

    protected GameObject ShowSwipeTutorialHint(Transform fromSwipe, Transform toSwipe)
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        ShowSkipButton(_tutorialManager.TutorialUiLayerTransform);
#endif
        var path = string.Format(TUTORIAL_SKINS_PATH, this.GetType().Name);
        _goTutorialHint = GameObject.Instantiate(Resources.Load<GameObject>(path), _tutorialManager.TutorialUiLayerTransform);
        _goTutorialHint.GetComponentInChildren<TutorHand>().MovePointerBetween2Positions(fromSwipe.position, toSwipe.position);
        return _goTutorialHint;
    }

    protected void TryToHideTutorialHint()
    {
        if (_goTutorialHint == null)
            return;

        GameObject.Destroy(_goTutorialHint);
        _goTutorialHint = null;
    }

    protected void InitCanvasButton(ref Button btn, UnityAction action, int sortingOrder, bool blockRaycast)
    {
        btn.onClick.AddListener(action);

        Canvas canvas = btn.gameObject.AddComponent<Canvas>();
        canvas.overrideSorting = true;
        canvas.sortingLayerName = "UI";
        canvas.sortingOrder = sortingOrder;

        btn.gameObject.AddComponent<AlwaysRaycastMask>();
        btn.gameObject.AddComponent<GraphicRaycaster>();

        if (blockRaycast)
        {
            _blockraycastImage = _tutorialManager.TutorialUiLayerTransform.gameObject.AddComponent<Image>();
            _blockraycastImage.color = Color.clear;
            _blockraycastImage.raycastTarget = true;
        }
    }

    protected void ActivateBlockRaycastImage(bool isActive)
    {
        if (isActive && _blockraycastImage == null)
        {
            _blockraycastImage = _tutorialManager.TutorialUiLayerTransform.gameObject.AddComponent<Image>();
            _blockraycastImage.color = Color.clear;
            _blockraycastImage.raycastTarget = true;
        }

        if (!isActive && _blockraycastImage != null)
        {
            GameObject.DestroyImmediate(_blockraycastImage);
            _blockraycastImage = null;
        }
    }

    protected void HiLightObject(GameObject hiLightObject, int sortingOrder)
    {
        bool isOn = false;
        if (!hiLightObject.activeSelf)
        {
            hiLightObject.SetActive(true);
            isOn = true;
        }

        Canvas canvas = hiLightObject.gameObject.AddComponent<Canvas>();
        if (canvas != null)
        {
            canvas.overrideSorting = true;
            canvas.sortingLayerName = "UI";
            canvas.sortingOrder = sortingOrder;
            hiLightObject.AddComponent<GraphicRaycaster>();
        }
        else
        {
            Debug.LogWarning($"Can not add Canvas on object {hiLightObject.gameObject.name}");
        }

        if (isOn)
        {
            hiLightObject.SetActive(false);
        }

    }

    protected void DisposeHiLightObject(GameObject hiLightObject)
    {
        GameObject.Destroy(hiLightObject.GetComponent<GraphicRaycaster>());
        GameObject.Destroy(hiLightObject.GetComponent<Canvas>());
    }

    protected void DisposeCanvasButton(ref Button btn, UnityAction action)
    {
        if (null != btn)
        {
            GameObject.Destroy(btn.GetComponent<AlwaysRaycastMask>());
            GameObject.Destroy(btn.GetComponent<GraphicRaycaster>());
            GameObject.Destroy(btn.GetComponent<Canvas>());

            btn.onClick.RemoveListener(action);
            btn = null;
        }

        if (_blockraycastImage != null)
        {
            GameObject.DestroyImmediate(_blockraycastImage);
        }
    }
}