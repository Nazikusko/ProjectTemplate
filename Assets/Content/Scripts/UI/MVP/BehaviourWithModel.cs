using System;
using UnityEngine;

public abstract class BehaviourWithModel<T> : MonoBehaviour, IObserver where T : Observable
{
    private T _model;

    public T Model
    {
        protected get
        {
            return _model;
        }
        set
        {
            OnApplyModel(value);

            _model = value;

            if (_model != null)
            {
                OnModelChanged(_model);
            }
        }
    }

    protected BehaviourWithModel()
    {
    }

    protected abstract void OnModelChanged(T model);

    protected virtual void OnApplyModel(T model)
    {
    }

    #region Observer implementation
    public void OnObjectChanged(Observable observable)
    {
        if (observable is T)
        {
            OnModelChanged((T)observable);
        }
        else
        {
            OnModelChanged(Model);
        }
    }
    #endregion
}