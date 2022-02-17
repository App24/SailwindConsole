using SailwindModdingHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SailwindConsole.Commands
{
    internal class WorldCoordsCommand : Command
    {
        public override string Name => "get_world_coords";

        public override void OnRun(List<string> args)
        {
            Vector3 position = Utils.PlayerTransform.position;
            ModConsole.Log($"Your current world position");
            ModConsole.Log($"X: {position.x}");
            ModConsole.Log($"Y: {position.y}");
            ModConsole.Log($"Z: {position.z}");
        }
    }
}
