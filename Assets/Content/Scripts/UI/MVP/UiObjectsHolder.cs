using System;
using System.Collections.Generic;
using UnityEngine;

public enum UiElementType
{
    Hud,
    Window,
    PopUp
}

public class UiObjectsHolder : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Transform _hudTransform;
    [SerializeField] private Transform _windowTransform;
    [SerializeField] private Transform _popupTransform;
    [SerializeField] private Transform _flyIconHolderTransform;
    [SerializeField] private Transform _tutorialTransform;

    private Dictionary<Type, IHud> _currentHudsList = new Dictionary<Type, IHud>();
    public Transform HudTransform => _hudTransform;
    public Transform WindowTransform => _windowTransform;
    public Transform PopupTransform => _popupTransform;
    public Transform FlyIconHolderTransform => _flyIconHolderTransform;
    public Transform TutorialTransform => _tutorialTransform;
    public Canvas Canvas => _canvas;


    public Transform GetRootObjectByUiType(UiElementType uiType)
    {
        switch (uiType)
        {
            case UiElementType.Hud: return _hudTransform;
            case UiElementType.Window: return _windowTransform;
            case UiElementType.PopUp: return _popupTransform;
        }

        return null;
    }

    public IHud TryGetUiElement(Type viewType)
    {
        foreach (var hud in _currentHudsList)
        {
            if (hud.Key == viewType) return hud.Value;
        }
        return null;
    }

    public void AddUiElement<T>(/*Type type,*/ T hud) where T : IHud
    {
        var type = hud.GetType();
        if (TryGetUiElement(type) != null)
            return;

        _currentHudsList.Add(type, hud);
    }

    public void DestroyUiElement<T>() where T : IHud
    {
        var type = typeof(T);
        var hud = TryGetUiElement(type);
        if (hud == null) return;

        _currentHudsList.Remove(type);

        Destroy(hud.transform.gameObject);
    }
}
