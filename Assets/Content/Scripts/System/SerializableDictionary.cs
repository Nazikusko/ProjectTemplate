using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableDictionary<TKey, TValue>
{
    [SerializeField] private List<SerializableKeyValuePair<TKey, TValue>> _items = new List<SerializableKeyValuePair<TKey, TValue>>();

    public Dictionary<TKey, TValue> ToDictionary()
    {
        var dict = new Dictionary<TKey, TValue>();
        foreach (var kvp in _items)
        {
            dict[kvp.Key] = kvp.Value;
        }
        return dict;
    }

    public void FromDictionary(Dictionary<TKey, TValue> dictionary)
    {
        _items.Clear();
        foreach (var kvp in dictionary)
        {
            _items.Add(new SerializableKeyValuePair<TKey, TValue>(kvp.Key, kvp.Value));
        }
    }

    public TValue GetValue(TKey key)
    {
        var dictionary = ToDictionary();
        return dictionary.ContainsKey(key) ? dictionary[key] : default(TValue);
    }
}

[Serializable]
public class SerializableKeyValuePair<TKey, TValue>
{
    public TKey Key;
    public TValue Value;

    public SerializableKeyValuePair(TKey key, TValue value)
    {
        Key = key;
        Value = value;
    }
}