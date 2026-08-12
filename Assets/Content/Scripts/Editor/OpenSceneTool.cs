using UnityEngine;

public class EditorDinoTool : MonoBehaviour
{
#if UNITY_EDITOR
    [UnityEditor.MenuItem("Tools/Scenes/EditorScene", false, 1)]
    public static void OpenMainScene()
    {
        UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/Content/Scenes/EditorScene.unity");
    }

    [UnityEditor.MenuItem("Tools/Scenes/GameScene", false, 2)]
    public static void OpenGameScene()
    {
        UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/Content/Scenes/GameScene.unity");
    }

    [UnityEditor.MenuItem("Tools/GameSave/ClearSave", false, 3)]
    public static void ClearSave()
    {
        SaveManager.ClearGameSave();
    }
#endif
}
