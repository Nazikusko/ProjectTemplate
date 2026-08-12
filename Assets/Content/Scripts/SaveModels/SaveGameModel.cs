using Newtonsoft.Json;
using System;

public class SaveGameModel
{
    public event Action<int> OnResourcesChanged;
    public event Action<int> OnUnlockedInventorySlotsChanged;

    public int Coins
    {
        get => _coins;
        set
        {
            _coins = value;
            OnResourcesChanged?.Invoke(_coins);
        }
    }

    public int UnlockedInventorySlotsCount
    {
        get => _unlockedInventorySlotsCount;
        set
        {
            _unlockedInventorySlotsCount = value;
            OnUnlockedInventorySlotsChanged?.Invoke(_unlockedInventorySlotsCount);
        }
    }

    public bool IsFirstRun;
    public int LocationIndex;
    public int StageIndex;
    public int HealthUpgradeLevel;
    public int DamageUpgradeLevel;
    public int TutorialStepIndex;

    [JsonProperty] private float _playerHealth;
    [JsonProperty] private int _coins;
    [JsonProperty] private int _unlockedInventorySlotsCount;

    public SaveGameModel()
    {
        LocationIndex = 1;
        StageIndex = 1;
        DamageUpgradeLevel = 1;
        HealthUpgradeLevel = 1;
        IsFirstRun = true;
    }
}