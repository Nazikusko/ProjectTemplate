using UnityEngine;
using Zenject;
using Debug = UnityEngine.Debug;

public sealed class TutorialManager
{
    public Transform TutorialUiLayerTransform => _uiObjectsHolder.TutorialTransform;
    public bool IsInTutorial => _model.TutorialStepIndex < _steps.Length;
    public bool IsShowingTutorialHint => _step != null && _step.IsShown;
    public int CurrentTutorialStepIndex => _model.TutorialStepIndex;
    public bool IsTutorialStarted => _model.TutorialStepIndex >= 0;

    [Inject] private UILinkManager _uiLinkManager;
    [Inject] private UiManager _uiManager;
    [Inject] private SaveGameModel _model;
    [Inject] private SaveManager _saveManager;
    [Inject] private DiContainer _container;

    private readonly TutorialStep[] _steps;
    private TutorialStep _step;
    private UiObjectsHolder _uiObjectsHolder;

    public TutorialManager(UiObjectsHolder uiObjectsHolder)
    {
        _uiObjectsHolder = uiObjectsHolder;
        _steps = new TutorialStep[]
        {
                //new TutorialStep1(), // equip right arm tutorial
                //new TutorialStep2(), // fight button tutorial
                //new TutorialStep3(), // Select card tutorial 1
                //new TutorialStep4(), // Select card tutorial 2
                //new TutorialStep5(), // equip left arm tutorial
                //new TutorialStep2(), // fight button tutorial
                //new TutorialStep6(), // merge tutorial
        };
    }

    public void StartTutorial()
    {
        _model.TutorialStepIndex = 0;
        TutorialStepComplete();
    }

    public void ContinueTutorial()
    {
        if (IsTutorialCompleted())
            return;
        TutorialStepComplete();
    }

    public bool IsTutorialCompleted()
    {
        return _model.TutorialStepIndex >= _steps.Length;
    }

    public void Dispose()
    {
        TryToCompleteStep();
    }

    private void OnTutorialStepComplete()
    {
        _model.TutorialStepIndex++;
        //_saveManager.SaveGameData(_model);

        TutorialStepComplete();
    }

    private void TutorialStepComplete()
    {
        TryToCompleteStep();

        if (_model.TutorialStepIndex >= _steps.Length)
        {
            return;
        }

        StartStep(_steps[_model.TutorialStepIndex]);
    }

    private void StartStep(TutorialStep step)
    {
        _step = step;
        _container.Inject(step);

        _step.OnComplete += OnTutorialStepComplete;
        _step.Initialize();

        Debug.Log($"Start tutorial {step}");
    }

    private void TryToCompleteStep()
    {
        if (_step == null)
            return;

        Debug.Log($"Complete tutorial {_step}");

        _step.OnComplete -= OnTutorialStepComplete;
        _step.Dispose();
    }

    public void SkipTutorial()
    {
        _model.TutorialStepIndex = _steps.Length;
        _saveManager.DebugSaveGameData(_model);
        TutorialStepComplete();
    }
}
