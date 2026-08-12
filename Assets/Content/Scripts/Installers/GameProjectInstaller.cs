using UnityEngine;
using Zenject;

public class GameProjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        var config = Resources.Load<Config>("Config");
        Container.Bind<Config>().FromInstance(config).AsSingle();

        var projectPrefabsHolder = Resources.Load<ProjectPrefabsHolder>("ProjectPrefabsHolder");
        Container.Bind<ProjectPrefabsHolder>().FromInstance(projectPrefabsHolder).AsSingle();

        var timer = new Timer();
        Container.Bind<Timer>().FromInstance(timer).AsSingle();

        var saveManager = new SaveManager();
        Container.Bind<SaveManager>().FromInstance(saveManager).AsSingle();

        var save = saveManager.LoadSaveGameData();
        Container.Bind<SaveGameModel>().FromInstance(save).AsSingle();

        var settings = saveManager.LoadSettings();
        Container.Bind<SettingsSaveModel>().FromInstance(settings).AsSingle();

        var soundSourcesHolder = Container.InstantiatePrefabForComponent<SoundSourcesHolder>(Resources.Load<SoundSourcesHolder>("SoundSourcesHolder"));
        soundSourcesHolder.transform.SetParent(null);
        DontDestroyOnLoad(soundSourcesHolder.gameObject);
        Container.Bind<SoundSourcesHolder>().FromInstance(soundSourcesHolder).AsSingle();

        Container.BindInterfacesAndSelfTo<AudioManager>().AsSingle();

        var bootManager = Container.InstantiatePrefabForComponent<InitBootManager>(Resources.Load<InitBootManager>("InitBootManager"));
        bootManager.transform.SetParent(null);
        DontDestroyOnLoad(bootManager.gameObject);

        Container.Bind<InitBootManager>().FromInstance(bootManager).AsSingle();
    }
}
