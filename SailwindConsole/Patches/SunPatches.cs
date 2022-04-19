using Crest;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityModManagerNet;
using Log = UnityModManagerNet.UnityModManager.Logger;

namespace SailwindConsole.Patches
{
    internal static class SunPatches
    {
        public static float defaultSeaLevel;
        public static GameObject ocean;
        public static float initialTimeStep;

        public static Wind wind;

        [HarmonyPatch(typeof(Sun), "Start")]
        private static class StartPatch
        {
            private static void Postfix(Sun __instance)
            {
                Log.Log("Starting console init");
                ModConsole.InitialiseConsole();
                ModConsole.HideConsole();
                ocean = GameObject.FindObjectOfType<OceanRenderer>().gameObject;
                defaultSeaLevel = ocean.transform.position.y;
                initialTimeStep = Time.fixedDeltaTime;
                wind = GameObject.FindObjectOfType<Wind>();
            }
        }
    }
}
