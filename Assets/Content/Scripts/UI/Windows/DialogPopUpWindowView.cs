using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogPopUpWindowModel : Observable
{
    public string DialogText;
}

public enum DialogPopUpWindowInputEnum
{
    Ok,
    Cancel,
}

public class DialogPopUpWindowView : BaseHudWithModel<DialogPopUpWindowModel, DialogPopUpWindowInputEnum>
{
    [SerializeField] private Button _okButton;
    [SerializeField] private Button _cancelButton;
    [SerializeField] private TMP_Text _messageText;

    protected override void OnEnable()
    {
        AddListener(_okButton,DialogPopUpWindowInputEnum.Ok);
        AddListener(_cancelButton, DialogPopUpWindowInputEnum.Cancel);
    }

    protected override void OnDisable()
    {
        RemoveListener(_okButton);
        RemoveListener(_cancelButton);
    }

    protected override void OnModelChanged(DialogPopUpWindowModel model)
    {
        _messageText.text = model.DialogText;
    }
}
