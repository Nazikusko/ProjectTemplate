using DG.Tweening;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public sealed class UiManager
{
    public const string VIEW_NAME = "View";
    public const string PATH = "UiElements";
    public const string TUTOR_PATH = "Tutor";

    public Transform FlyIconHolderTransform => _uiObjectsContainer.FlyIconHolderTransform;
    public Canvas Canvas => _uiObjectsContainer?.Canvas;

    [Inject] private DiContainer _container;
    [Inject] private SceneObjectsHolder _sceneObjectsHolder;
    [Inject] private CameraManager _cameraManager;

    private readonly List<Mediator> _additionalHuds;
    private UiObjectsHolder _uiObjectsContainer;
    private TutorUi _currentTutorUi;

    public UiManager(UiObjectsHolder holder)
    {
        _additionalHuds = new List<Mediator>();
        _uiObjectsContainer = holder;
    }
    
    private Mediator ShowAdditional(UiElementType uiType, Type type, params object[] args)
    {
        var mediator = (Mediator)Activator.CreateInstance(type, args);
        _container.Inject(mediator);
        _additionalHuds.Add(mediator);

        var hudType = mediator.ViewType;
        var hudView = CreateHud(uiType, hudType);

        if (hudView.transform.parent == _uiObjectsContainer.GetRootObjectByUiType(uiType))
        {
            hudView.transform.SetAsLastSibling();
        }

        mediator.Mediate(hudView);
        mediator.InternalShow();

        return mediator;
    }

    public T ShowAdditional<T>(UiElementType uiType, params object[] args) where T : Mediator
    {
        return (T)ShowAdditional(uiType, typeof(T), args);
    }

    public void HideAllAdditionals()
    {
        for (int i = _additionalHuds.Count - 1; i >= 0; i--)
        {
            var hud = _additionalHuds[i];
            hud.InternalHide();
            hud.Unmediate();
            _additionalHuds.RemoveAt(i);
        }
    }

    public void HideAdditional<T>() where T : Mediator
    {
        for (int i = _additionalHuds.Count - 1; i >= 0; i--)
        {
            var hud = _additionalHuds[i];

            if (!(hud is T))
                continue;

            hud.InternalHide();
            hud.Unmediate();
            _additionalHuds.RemoveAt(i);
        }
    }

    public bool IsOpened<T>()
    {
        return _additionalHuds.Exists(type => type is T);
    }

    public T GetHud<T>() where T : Mediator
    {
        return _additionalHuds.Find(type => type is T) as T;
    }

    public void ForceShow<T>(UiElementType uiType) where T : IHud
    {
        var hud = CreateHud(uiType, typeof(T));

        if (hud == null)
            return;

        hud.IsActive = true;
    }

    private IHud CreateHud(UiElementType uiType, Type viwType)
    {
        var hudView = _uiObjectsContainer.TryGetUiElement(viwType);

        if (null == hudView)
        {
            string fileName = viwType.Name.Replace(VIEW_NAME, string.Empty);
            var prefab = Resources.Load<GameObject>(Path.Combine(PATH, fileName));
            if (null == prefab)
            {
                Debug.LogError("Can't find hud " + Path.Combine(PATH, fileName));
                return null;
            }

            hudView = _container.InstantiatePrefab(prefab, _uiObjectsContainer.GetRootObjectByUiType(uiType)).GetComponent<IHud>();
            _uiObjectsContainer.AddUiElement(hudView);
        }

        hudView.transform.SetAsFirstSibling();

        return hudView;
    }
}