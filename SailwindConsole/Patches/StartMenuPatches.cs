using HarmonyLib;
using SailwindModdingHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SailwindConsole.Patches
{
    internal static class StartMenuPatches
    {

        [HarmonyPatch(typeof(StartMenu), "LateUpdate")]
        private static class LateUpdatePatch
        {
            private static void Postfix(StartMenu __instance)
            {
                if (Main.enabled)
                {
                    if (Input.GetKeyDown(KeyCode.BackQuote) && !ModConsole.consoleInput.isFocused && Utils.GamePaused)
                    {
                        ModConsole.ToggleConsole();
                    }
                }
            }
        }

        [HarmonyPatch(typeof(StartMenu), "GameToSettings")]
        private static class ConsoleOpenPatch
        {
            private static void Postfix(StartMenu __instance)
            {
                if (Main.enabled)
                {
                    ModConsole.ShowConsole();
                }
            }
        }

        [HarmonyPatch(typeof(StartMenu), "SettingsToGame")]
        private static class ConsoleClosePatch
        {
            private static void Postfix(StartMenu __instance)
            {
                if (Main.enabled)
                {
                    ModConsole.HideConsole();
                }
            }
        }
    }
}
