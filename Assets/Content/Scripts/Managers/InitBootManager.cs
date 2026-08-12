using System;
using DG.Tweening;
using IngameDebugConsole;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class InitBootManager : MonoBehaviour
{
    [SerializeField] private GameObject _loadScreen;
    [SerializeField] private TMP_Text _progressText;
    [SerializeField] private FillPanel _fillPanel;

    private static DebugLogManager _inGameDebugConsole;

    [Inject] private Timer _timer;
    [Inject] private SaveGameModel _save;
    [Inject] private SaveManager _saveManager;

    [UnityEngine.Scripting.Preserve]
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void RootInitialize()
    {
        _inGameDebugConsole = Instantiate(Resources.Load<DebugLogManager>("InGameDebugConsole"));

#if DEVELOPMENT_BUILD
        _inGameDebugConsole.gameObject.SetActive(true);
        Debug.developerConsoleVisible = false;
        Debug.developerConsoleEnabled = false;
#else
        _inGameDebugConsole.gameObject.SetActive(false);
#endif
    }

    private void Awake()
    {
        _fillPanel.Init();
        _fillPanel.ShowFill();
        
        Application.targetFrameRate = 60;
        DOTween.SetTweensCapacity(500, 125);

        SceneManager.sceneLoaded += OnSceneLoaded;

        var sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == nameof(SceneType.InitBoot))
        {
            LoadScene(SceneType.MainMenu, showFill: false);
        }
        else
        {
            _fillPanel.HideFill(0.4f);
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadType)
    {
        if (scene.name == nameof(SceneType.InitBoot)) return;

        _loadScreen.gameObject.SetActive(false);
        _fillPanel.HideFill(0.6f);
    }

    public void ShowFill(float duration, Action onComplete = null)
    {
        _fillPanel.ShowFill(duration, onComplete);
    }

    public void HideFill(float duration, Action onComplete = null)
    {
        _fillPanel.HideFill(duration, onComplete);
    }

    public void LoadScene(SceneType sceneType, bool showFill = true)
    {
        if (showFill)
        {
            _fillPanel.ShowFill(0.25f, LoadSceneCallback);
        }
        else
        {
            LoadSceneCallback();
        }

        void LoadSceneCallback()
        {
            DOTween.KillAll();
            if (sceneType == SceneType.GameScene)
            {
                StartCoroutine(LoadAsync($"{sceneType}_1"));
            }
            else
            {
                StartCoroutine(LoadAsync(sceneType.ToString()));
            }
        }
    }

    private IEnumerator LoadAsync(string sceneName)
    {
        _loadScreen.gameObject.SetActive(true);

        yield return null;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);

            _progressText.text = $"{(int)(progress * 100)}%";

            if (progress >= 1f)
                asyncLoad.allowSceneActivation = true;

            yield return null;
        }
    }

    private void OnApplicationQuit()
    {
        _saveManager.SaveGameData(_save);
    }

    private void OnApplicationPause(bool paused)
    {
        if (!paused)
            return;
        _saveManager.SaveGameData(_save);
    }

    void Update()
    {
        _timer.Update();
    }

    void LateUpdate()
    {
        _timer.LateUpdate();
    }

    void FixedUpdate()
    {
        _timer.FixedUpdate();
    }
}

public enum SceneType
{
    InitBoot,
    GameScene,
    Lobby,
    MainMenu,
}