using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    [SerializeField] protected UiObjectsHolder _uiObjectsHolder;
    [SerializeField] protected SceneObjectsHolder _sceneObjectsHolder;
    [SerializeField] protected CameraManagerObjectsHolder _cameraManagerObjectsHolder;

    [Inject] Timer _timer;

    public override void InstallBindings()
    {
        Container.Bind<SceneObjectsHolder>().FromInstance(_sceneObjectsHolder).AsSingle();
        Container.Bind<UiManager>().AsSingle().WithArguments(_uiObjectsHolder);

        Container.Bind<InputManager>().AsSingle().OnInstantiated<InputManager>((ctx, inputManager) =>
        {
            inputManager.Init(_uiObjectsHolder.Canvas.GetComponent<GraphicRaycaster>(), _timer);
        });
        Container.BindInterfacesAndSelfTo<CameraManager>().AsSingle().WithArguments(_cameraManagerObjectsHolder);
        Container.BindInterfacesAndSelfTo<UILinkManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<TutorialManager>().AsSingle().WithArguments(_uiObjectsHolder);
    }
}
