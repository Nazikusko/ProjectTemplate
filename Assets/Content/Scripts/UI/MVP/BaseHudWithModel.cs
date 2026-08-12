using System;
using System.Collections.Generic;
using UnityEngine.UI;

public abstract class BaseHudWithModel<TModel, TInput> : BaseHud, IObserver where TModel : Observable where TInput : Enum
{
    public event Action<TInput, object> OnInputReceived;

    private TModel _model;

    public TModel Model
    {
        protected get
        {
            return _model;
        }
        set
        {
            if (null != _model)
            {
                _model.RemoveObserver(this);
            }

            OnApplyModel(value);

            _model = value;

            if (null != _model)
            {
                _model.AddObserver(this);
                OnModelChanged(_model);
            }
        }
    }

    private List<ButtonListener<TInput>> _buttonListeners = new List<ButtonListener<TInput>>();

    protected BaseHudWithModel()
    {
    }

    protected abstract void OnModelChanged(TModel model);

    protected virtual void OnApplyModel(TModel model)
    {
    }

    #region Observer implementation
    public void OnObjectChanged(Observable observable)
    {
        if (observable is TModel)
        {
            OnModelChanged((TModel)observable);
        }
        else
        {
            OnModelChanged(Model);
        }
    }
    #endregion

    protected void AddListener(Button button, TInput inputEnum, object arg = null)
    {
        _buttonListeners.Add(new ButtonListener<TInput>(button, inputEnum, arg, OnInputReceivedHandler));
    }

    protected void RemoveListener(Button button)
    {
        for (int i = _buttonListeners.Count - 1; i >= 0; i--)
        {
            if (_buttonListeners[i].Button == button)
            {
                _buttonListeners[i].Dispose();
                _buttonListeners.RemoveAt(i);
            }
        }
    }

    private void OnInputReceivedHandler(TInput action, object arg)
    {
        OnInputReceived?.Invoke(action, arg);
    }
}