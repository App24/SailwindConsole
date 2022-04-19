using HarmonyLib;
using SailwindModdingHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SailwindConsole.Commands
{
    internal class ListSpawnableObjectsCommand : Command
    {
        public override string Name => "spawnableObjects";

        public override string Description => "List all spawnable objects";

        public override void OnRun(List<string> args)
        {
            Dictionary<string, GameObject> prefabs = Traverse.Create(typeof(Prefabs)).Field("prefabs").GetValue<Dictionary<string, GameObject>>();

            foreach (var item in prefabs)
            {
                UnityModManagerNet.UnityModManager.Logger.Log(item.Key);
            }
        }
    }
}
