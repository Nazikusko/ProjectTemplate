using UnityEngine;

public interface IHud
{
    bool IsActive
    {
        set;
    }

    Transform transform { get; }
}