using System;
using UnityEngine.UI;

public sealed class ButtonListener<T> where T : Enum
{
    private Button _button;
    private readonly T _inputType;
    private readonly object _arg;
    private Action<T, object> _inputReceived;

    public Button Button => _button;

    public ButtonListener(Button button, T inputType, object arg, Action<T, object> inputReceived)
    {
        _button = button;
        _inputType = inputType;
        _arg = arg;
        _inputReceived = inputReceived;

        _button.onClick.AddListener(OnClicked);
    }

    public void Dispose()
    {
        _button.onClick.RemoveListener(OnClicked);
    }

    private void OnClicked()
    {
        _inputReceived.Invoke(_inputType, _arg);
    }
}
