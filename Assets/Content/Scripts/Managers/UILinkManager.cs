using System;
using System.Collections.Generic;
using UnityEngine;


public enum UILinkKey
{
    FightButton,
    BattleSwipeObject,
    BattleRightAttackButtonHolder,
    UpgradeEquipButton,
    CardSlotHolder1,
    CardSlotHolder2,
}

public sealed class UILinkManager : IDisposable
{
    public Action<UILinkKey> OnKeyEdded;

    private readonly Dictionary<UILinkKey, Component> _componentsPool;

    public UILinkManager()
    {
        _componentsPool = new Dictionary<UILinkKey, Component>();
    }

    public void Dispose()
    {
        _componentsPool.Clear();
    }

    public void Link(UILinkKey key, Component component)
    {
        _componentsPool[key] = component;
        OnKeyEdded?.Invoke(key);
    }

    public void Unlink(UILinkKey key)
    {
        _componentsPool.Remove(key);
    }

    public T Get<T>(UILinkKey key) where T : Component
    {
        return (T)_componentsPool[key];
    }

    public bool Exists(UILinkKey key)
    {
        return _componentsPool.ContainsKey(key);
    }
}