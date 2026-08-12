using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class ComponentPoolFactory : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private int _count;
    [SerializeField] private Transform _content;
    [SerializeField] private Transform _poolStorage;
    [SerializeField] private bool _isInject;

    private readonly HashSet<GameObject> _instances;
    private Queue<GameObject> _pool;

    [Inject] DiContainer _container;

    public Transform Content => _content;

    public ComponentPoolFactory()
    {
        _instances = new HashSet<GameObject>();
        _pool = new Queue<GameObject>();
    }

    public int CountInstances => _instances.Count;

    private void Awake()
    {
        if (_instances.Count > 0)
            return;

        for (int i = 0; i < _count; i++)
        {
            Get<Transform>();
        }
        ReleaseAllInstances();
    }

    public T Get<T>() where T : Component
    {
        return Get<T>(_instances.Count);
    }

    public T Get<T>(int sublingIndex) where T : Component
    {
        bool isNewInstance = false;
        if (_pool.Count == 0)
        {
            GameObject result = !_isInject ? Instantiate(_prefab) : _container.InstantiatePrefab(_prefab);

            if (result == null)
                return null;

            _pool.Enqueue(result);
            isNewInstance = true;
        }

        T resultComponent = _pool.Dequeue().GetComponent<T>();
        if (resultComponent == null)
        {
            return null;
        }

        var item = resultComponent.gameObject;
        var itemTransform = resultComponent.transform;
        if (isNewInstance || (_poolStorage != null && _poolStorage != _content))
        {
            itemTransform.SetParent(_content);
        }

        _instances.Add(item);

        if (!item.activeSelf)
        {
            item.SetActive(true);
        }

        if (itemTransform.GetSiblingIndex() != sublingIndex)
        {
            itemTransform.SetSiblingIndex(sublingIndex);
        }

        return resultComponent;
    }

    public void Release<T>(T component) where T : Component
    {
        var go = component.gameObject;
        if (_instances.Contains(go))
        {
            go.SetActive(false);
            if (_poolStorage != null)
            {
                go.transform.SetParent(_poolStorage, false);
            }
            _pool.Enqueue(go);
            _instances.Remove(go);
        }
    }

    public void ReleaseAllInstances()
    {
        foreach (GameObject instance in _instances)
        {
            instance.SetActive(false);
            if (_poolStorage != null)
            {
                instance.transform.SetParent(_poolStorage, false);
            }
            _pool.Enqueue(instance);
        }
        _instances.Clear();
    }

    public void PutInstancesToPool()
    {
        _pool = new Queue<GameObject>(_instances.Union(_pool));
        _instances.Clear();
    }

    public void HideUnusedInstances()
    {
        foreach (GameObject instance in _pool)
        {
            instance.SetActive(false);
        }
    }

    public void Dispose()
    {
        ReleaseAllInstances();

        foreach (GameObject gameObject in _pool)
        {
            GameObject.Destroy(gameObject);
        }
        _pool.Clear();
    }
}