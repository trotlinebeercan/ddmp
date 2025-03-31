using BepInEx.Configuration;
using DungeonDrafters.Settings;
using HarmonyLib;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace ddmp;

public class GameSpeedPatch : IPatch
{
    static internal bool EnableModifiedSpeedState = false;

    [HarmonyPatch(typeof(GameSetting), nameof(GameSetting.EnableModifiedSpeed))]
    [HarmonyPrefix]
    static void EnableModifiedSpeed()
    {
        Plugin.L($"GameSetting.EnableModifiedSpeed hook hit!");
        EnableModifiedSpeedState = !EnableModifiedSpeedState;
    }

    [HarmonyPatch(typeof(GameSetting), nameof(GameSetting.ResetModifiedSpeed))]
    [HarmonyPrefix]
    static void ResetModifiedSpeed(ref bool __runOriginal)
    {
        Plugin.L($"GameSetting.ResetModifiedSpeed hook hit!");
        __runOriginal = !EnableModifiedSpeedState;
    }

    public void Initialize()
    {
        ConfigManager.Instance.StartSection("Time Management")
            .Create(EnableToggleSpeedActionId, "Enable FF Speed as Toggle", true,
                "Changes push-to-fast-forward to a toggle.",
                null,
                new ConfigurationManagerAttributes { IsAdvanced = true }
            )
        .EndSection("Time Management");
    }

    internal static string EnableToggleSpeedActionId = "ToggleSpeedAction";
}
