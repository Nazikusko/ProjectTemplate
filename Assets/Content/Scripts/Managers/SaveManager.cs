using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class SaveManager
{
    private const string SAVE_KEY = "TEMPLATE_SAVE_KEY";
    private const string SAVE_SETTINGS_KEY = "TEMPLATE_SAVE_SETTINGS_KEY";

    private JsonSerializerSettings _settings;

    public SaveManager()
    {
        _settings = new JsonSerializerSettings();
        _settings.Converters.Add(new Vector3JsonConverter());
        _settings.Converters.Add(new ColorJsonConverter());
    }

    public async Task<T> LoadDataFromFile<T>(string path) where T : new()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(path))
        {
            var operation = request.SendWebRequest();
            while (!operation.isDone) await Task.Yield();

            if (request.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    var result = JsonConvert.DeserializeObject<T>(request.downloadHandler.text);
                    return result;
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                    return new T();
                }
            }
            else
            {
                Debug.LogError($"Result: {request.result}\n");
                Debug.LogError($"File not  loaded: {path}");
                return new T();
            }
        }
    }

    public void SaveDataToFile<T>(T dataModel, string path)
    {
        string json = JsonConvert.SerializeObject(dataModel, Formatting.Indented);

        File.WriteAllText(path, json);
    }
    
    public SettingsSaveModel LoadSettings()
    {
        if (!PlayerPrefs.HasKey(SAVE_SETTINGS_KEY))
        {
            return new SettingsSaveModel();
        }

        string loadJsonData = PlayerPrefs.GetString(SAVE_SETTINGS_KEY);
        try
        {
            var result = JsonConvert.DeserializeObject<SettingsSaveModel>(loadJsonData, _settings);
            return result;
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            return new SettingsSaveModel();
        }
    }

    public SaveGameModel LoadSaveGameData()
    {
        if (!PlayerPrefs.HasKey(SAVE_KEY))
        {
            return new SaveGameModel();
        }

        string loadJsonData = PlayerPrefs.GetString(SAVE_KEY);
        try
        {
            var result = JsonConvert.DeserializeObject<SaveGameModel>(loadJsonData);
            return result;
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            return new SaveGameModel();
        }
    }

    public void SaveGameData(SaveGameModel dataModel)
    {
#if UNITY_EDITOR
        return; 
#endif
        string json = JsonConvert.SerializeObject(dataModel);

        PlayerPrefs.SetString(SAVE_KEY, json);
        PlayerPrefs.Save();
    }

    public void DebugSaveGameData(SaveGameModel dataModel)
    {
        string json = JsonConvert.SerializeObject(dataModel);

        PlayerPrefs.SetString(SAVE_KEY, json);
        PlayerPrefs.Save();
    }

    public static void ClearGameSave()
    {
        if (!PlayerPrefs.HasKey(SAVE_KEY)) return;

        PlayerPrefs.DeleteKey(SAVE_KEY);
    }
}