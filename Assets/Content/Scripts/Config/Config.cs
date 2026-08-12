using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

[CreateAssetMenu(fileName = "Config", menuName = "MyData/Config", order = 0)]
public class Config : SerializedScriptableObject
{
    private const string _spreadsheetId = "1-BprFWp433FlLG_Ief_kWpEogQjw-bRDJVks9hYRt3Y";
    private const string _credentialsFileName = "fantasyhorizons-1c68661953fc.json";

    [field: SerializeField] public float PlayerMeleeAttackDistance { get; private set; }
    [field: SerializeField, OdinSerialize] public Dictionary<int, int> Test { get; private set; }

    void OnValidate()
    {
        ClearCache();
    }

    [Button]
    public void ClearCache()
    {

    }

    [Button]
    public async void DownloadRemoteSheetsData()
    {
        Dictionary<ConfigPages, string> pagesRequests = new Dictionary<ConfigPages, string>()
        {
            {ConfigPages.Equipment, "Equipment!A2:J100"},
            {ConfigPages.CardsConfig,"CardsConfig!A2:P100"},
            {ConfigPages.InventoryUnlockCellsCosts, "InventoryUnlockCellsCosts!A2:B30" }
        };

        string path = string.Empty;

#if UNITY_ANDROID && !UNITY_EDITOR
    path = Path.Combine(Application.streamingAssetsPath, _credentialsFileName);
#else
        path = "file://" + Path.Combine(Application.streamingAssetsPath, _credentialsFileName);
#endif

        using UnityWebRequest requestCredential = UnityWebRequest.Get(path);

        var operation = requestCredential.SendWebRequest();

        while (!operation.isDone)
        {
            await Task.Yield();
        }

        if (requestCredential.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"Error loading google credential file: {requestCredential.error}");
            return;
        }

        GoogleCredential credential = GoogleCredential.FromJson(requestCredential.downloadHandler.text);

        var service = new SheetsService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = "Unity Google Sheets API"
        });

        foreach (var pagesRequest in pagesRequests)
        {

            ValueRange response;
            try
            {
                SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(_spreadsheetId, pagesRequest.Value);
                response = await request.ExecuteAsync();
            }
            catch (Exception e)
            {
                Debug.LogError($"Error reading Google Sheets: {e.Message}");
                return;
            }

            ParseData(pagesRequest.Key ,response.Values);
        }
        Debug.Log("Google Sheets data downloaded and parsed successfully.");
    }

    private void ParseData(ConfigPages page, IList<IList<object>> data)
    {
        foreach (var row in data)
        {

            //EquipmentType = (EquipmentType)Enum.Parse(typeof(EquipmentType), (row[0].ToString())),
            //EquipmentSlots = new List<EquipmentSlot>(),
            //Level = int.Parse(row[2].ToString()),
            //DisplayedName = (string)row[3],
            //PointsForMerge = int.Parse(row[4].ToString()),
            //PowerScore = Mathf.RoundToInt(float.Parse(row[9].ToString())),
        }
    }

    enum ConfigPages
    {
        Equipment,
        CardsConfig,
        InventoryUnlockCellsCosts
    }
}