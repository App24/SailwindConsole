using Crest;
using SailwindConsole.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SailwindConsole.Commands
{
    internal class SeaLevelCommand : Command
    {
        public override string Name => "setSeaLevel";
        public override int MinArgs => 1;
        public override string Usage => "<sea level>";

        public override string Description => "Change the sea level";

        public override void OnRun(List<string> args)
        {
            float.TryParse(args[0], out float level);
            SunPatches.ocean.transform.position = new Vector3(SunPatches.ocean.transform.position.x, SunPatches.defaultSeaLevel + level, SunPatches.ocean.transform.position.z);
            ModConsoleLog.Log($"Set sea level to {level}");
        }
    }
}
