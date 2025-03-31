using System;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;

namespace ddmp;

[BepInPlugin(PluginInfo.PackageId, PluginInfo.Title, PluginInfo.Version)]
[BepInProcess(PluginInfo.ExecutableTarget)]
public class Plugin : BasePlugin
{
    internal static new ManualLogSource Log;
    internal static Harmony Harmony;

    public override void Load()
    {
        Log = base.Log;
        Log.LogInfo($"Hello world from {PluginInfo.PackageId}! (version: {PluginInfo.Version})");
        Log.LogInfo($"Loading...");

        Harmony = new Harmony(PluginInfo.PackageId);
        ConfigManager.Instance.Initialize(Config);
        ApplyPatches();

        Log.LogInfo("Patches applied!");
    }

    public override bool Unload()
    {
        Log.LogInfo("Unloading Harmony. Goodbye!");
        Harmony.UnpatchSelf();
        return false;
    }

    private void ApplyPatches()
    {
        ApplyPatch(new GameSpeedPatch());
    }

    private void ApplyPatch(IPatch patch)
    {
        Log.LogInfo($"Patching {patch.GetType().Name}...");
        patch.Initialize();
        Harmony.PatchAll(patch.GetType());
    }

    internal static void L(string message) => Log.LogMessage(message);
    internal static void E(string message) => Log.LogError(message);
    internal static void W(string message) => Log.LogWarning(message);
    internal static void D(string message) => Log.LogDebug(message);
}
