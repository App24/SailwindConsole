using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Log = UnityModManagerNet.UnityModManager.Logger;

namespace SailwindConsole.Patches
{
    internal static class SunPatches
    {
        [HarmonyPatch(typeof(Sun), "Start")]
        private static class StartPatch
        {
            private static void Postfix(Sun __instance)
            {
                Log.Log("Starting console init");
                ModConsole.InitialiseConsole();
                ModConsole.HideConsole();
            }
        }
    }
}
