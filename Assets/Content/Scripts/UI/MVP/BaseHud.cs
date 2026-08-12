using UnityEngine;

public abstract class BaseHud : MonoBehaviour, IHud
{
    public bool IsActive
    {
        set
        {
            if (null == gameObject)
                return;

            gameObject.SetActive(value);
        }
    }

    protected abstract void OnEnable();
    protected abstract void OnDisable();
}