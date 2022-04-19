using HarmonyLib;
using SailwindModdingHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SailwindConsole.Patches
{
    internal static class PortPatches
    {
        [HarmonyPatch(typeof(Port), "Update")]
        public static class UpdatePatch
        {
            [HarmonyPrefix]
            public static bool Prefix()
            {
                if (!Main.enabled) return true;
                return !Utils.GamePaused;
            }
        }
    }
}
