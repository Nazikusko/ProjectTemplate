public class SettingsSaveModel
{
    public bool IsMusicOn => MusicVolume > 0.01f;
    public bool IsSfxOn => SfxVolume > 0.01f;

    public float MusicVolume;
    public float SfxVolume;
    
    public SettingsSaveModel()
    {
        SetSettingsToDefault();
    }

    private void SetSettingsToDefault()
    {
        MusicVolume = 1f;
        SfxVolume = 0.7f;
    }
}