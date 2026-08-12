using System;
using System.Collections.Generic;

//модель
public interface IObservable
{
    void SetChanged();
    void AddObserver(IObserver observer);
    void RemoveObserver(IObserver observer);
}

[Serializable]
public abstract class Observable : IObservable
{
    private readonly List<IObserver> _observers;

    protected Observable()
    {
        _observers = new List<IObserver>();
    }

    public List<IObserver> Observers => _observers;

    public void SetChanged()
    {
        if (_observers.Count == 0)
            return;

        foreach (IObserver observer in _observers)
        {
            observer.OnObjectChanged(this);
        }
    }

    public void AddObserver(IObserver observer)
    {
        var index = _observers.IndexOf(observer);

        if (index != -1)
            return;

        _observers.Add(observer);
        OnObserversChanged(_observers.Count);
    }

    public void RemoveObserver(IObserver observer)
    {
        var index = _observers.IndexOf(observer);
        if (index == -1)
            return;

        _observers.RemoveAt(index);
        OnObserversChanged(_observers.Count);
    }

    public void Clear()
    {
        _observers.Clear();
        OnObserversChanged(_observers.Count);
    }

    protected virtual void OnObserversChanged(int count)
    {
    }
}
