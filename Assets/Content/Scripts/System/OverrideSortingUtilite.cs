using UnityEngine;
using UnityEngine.UI;

public static class OverrideSortingUtilite 
{
    public static void AddOverrideSorting(Transform transform, int sortingOrder = 2)
    {
        bool isOn = false;
        if (!transform.gameObject.activeSelf)
        {
            transform.gameObject.SetActive(true);
            isOn = true;
        }

        Canvas canvas = transform.gameObject.AddComponent<Canvas>();
        canvas.overrideSorting = true;
        canvas.sortingOrder = sortingOrder;
        transform.gameObject.AddComponent<GraphicRaycaster>();

        if (isOn)
        {
            transform.gameObject.SetActive(false);
        }
    }

    public static void RemoveOverrideSorting(Transform transform)
    {
        if (transform.TryGetComponent<GraphicRaycaster>(out var raycaster))
        {
            GameObject.Destroy(raycaster);
        }
        if (transform.TryGetComponent<Canvas>(out var canvas))
        {
            GameObject.Destroy(canvas);
        }
    }
}
