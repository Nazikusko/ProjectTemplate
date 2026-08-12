using System;
using Zenject;

public class DialogPopUpWindowMediatorParameters
{
    public Action OkButtonAction;
    public Action CancelButtonAction;
    public string Message;
}

public class DialogPopUpWindowMediator : Mediator<DialogPopUpWindowView>
{
    private DialogPopUpWindowModel _model;

    private Action _okButtonAction;
    private Action _cancelButtonAction;
    private string _message;
    [Inject] private UiManager _uiManager;

    public DialogPopUpWindowMediator(DialogPopUpWindowMediatorParameters parameters)
    {
        _okButtonAction = parameters.OkButtonAction;
        _cancelButtonAction = parameters.CancelButtonAction;
        _message = parameters.Message;
    }

    protected override void Show()
    {
        //_uiManager = ServiceLocator.Get<UiManager>();
        _model = new DialogPopUpWindowModel()
        {
            DialogText = _message,
        };

        View.Model = _model;
        View.OnInputReceived += OnInputReceived;
    }

    private void OnInputReceived(DialogPopUpWindowInputEnum input, object arg2)
    {
        switch (input)
        {
            case DialogPopUpWindowInputEnum.Ok:
                _okButtonAction?.Invoke();
                _uiManager.HideAdditional<DialogPopUpWindowMediator>();
                break;

            case DialogPopUpWindowInputEnum.Cancel:
                _cancelButtonAction?.Invoke();
                _uiManager.HideAdditional<DialogPopUpWindowMediator>();
                break;
        }
    }

    protected override void Hide()
    {
        View.OnInputReceived -= OnInputReceived;
    }
}
