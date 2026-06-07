using HarmonyLib;
using System;
using System.Linq;
using System.Reflection;

namespace PinIt.Patches
{
    public static class ToolbarPatch
    {
        private static MethodInfo s_PinItToggle;

        public static void Apply(Harmony harmony)
        {
            s_PinItToggle = typeof(ToolbarPatch).GetMethod(nameof(OnUpdatePostfix), BindingFlags.Static | BindingFlags.NonPublic);

            TryPatch(harmony, "Game.UI.InGame.ToolbarUISystem");
            TryPatch(harmony, "Game.UI.InGame.ToolbarBottomUISystem");
            TryPatch(harmony, "Game.UI.InGame.ModsMenuPanel");
        }

        private static void TryPatch(Harmony harmony, string fullTypeName)
        {
            try
            {
                var type = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a =>
                    {
                        try { return a.GetTypes(); }
                        catch { return Type.EmptyTypes; }
                    })
                    .FirstOrDefault(t => t.FullName == fullTypeName);

                if (type == null)
                {
                    Mod.log.Info($"[PinIt] ToolbarPatch: {fullTypeName} not found");
                    return;
                }

                // Try OnUpdate first, then OnCreate — whichever exists on the declared type
                var target = type.GetMethod("OnUpdate", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                          ?? type.GetMethod("OnCreate", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

                if (target == null)
                {
                    Mod.log.Info($"[PinIt] ToolbarPatch: no OnUpdate/OnCreate on {type.Name}");
                    return;
                }

                harmony.Patch(target, postfix: new HarmonyMethod(s_PinItToggle));
                Mod.log.Info($"[PinIt] ToolbarPatch: patched {type.Name}.{target.Name}");
            }
            catch (Exception ex)
            {
                Mod.log.Error($"[PinIt] ToolbarPatch.TryPatch({fullTypeName}) failed: {ex.Message}");
            }
        }

        private static void OnUpdatePostfix(object __instance) { }
    }
}
