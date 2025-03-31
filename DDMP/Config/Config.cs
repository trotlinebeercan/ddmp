using System.Collections.Generic;
using System.Diagnostics;
using BepInEx.Configuration;
using ddmp;

public class ConfigManager : BaseSingleton<ConfigManager>
{
    internal ConfigFile Config;
    internal Dictionary<string, ConfigDefinition> Entries;

    internal int CurrentOrderIndex;
    internal string CurrentSection;

    public void Initialize(ConfigFile inConfig)
    {
        Config = inConfig;
        Entries = new Dictionary<string, ConfigDefinition>();
        CurrentOrderIndex = 100;
        CurrentSection = "";
    }

    public ConfigManager StartSection(string section)
    {
        Debug.Assert(CurrentSection == "", "StartSection failed, current section was non-null");
        CurrentSection = section;
        return this;
    }

    public void EndSection(string section)
    {
        Debug.Assert(section == CurrentSection, "EndSection failed, current section was not equal to argument");
        CurrentSection = "";
    }

    public ConfigManager Create<T>(
        string uuid, string text, T defaultValue, string description,
        AcceptableValueBase acceptableValues,
        ConfigurationManagerAttributes tags,
        System.EventHandler onSettingChanged = null
    )
    {
        Debug.Assert(CurrentSection != "", "Create failed, current section was null");

        ConfigDefinition newConfDef = new ConfigDefinition(CurrentSection, uuid);
        Entries.Add(uuid, newConfDef);

        // order them in the same order they are initialized
        tags.Order = --CurrentOrderIndex;
        tags.DispName = text;

        ConfigDescription newConfDesc = new ConfigDescription(description, acceptableValues, tags);
        ConfigEntry<T> newConf = Config.Bind(newConfDef, defaultValue, newConfDesc);
        newConf.SettingChanged += onSettingChanged;
        return this;
    }

    public ConfigEntry<T> Get<T>(string key)
    {
        return Config[Entries[key]] as ConfigEntry<T>;
    }

    public T GetValue<T>(string key)
    {
        return (T)Config[Entries[key]].BoxedValue;
    }
}
