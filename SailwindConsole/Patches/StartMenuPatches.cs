using HarmonyLib;
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
        static bool paused;

        [HarmonyPatch(typeof(StartMenu), "LateUpdate")]
        private static class LateUpdatePatch
        {
            private static void Postfix(StartMenu __instance)
            {
                if (Main.enabled)
                {
                    if (Input.GetKeyDown(KeyCode.BackQuote) && !ModConsole.consoleInput.isFocused && paused)
                    {
                        ModConsole.ToggleConsole();
                        ModConsole.consoleInput.DeactivateInputField();
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
                    paused = true;
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
                    paused = false;
                }
            }
        }
    }
}
