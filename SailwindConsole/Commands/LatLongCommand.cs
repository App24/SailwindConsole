using SailwindModdingHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SailwindConsole.Commands
{
    internal class LatLongCommand : Command
    {
        public override string Name => "get_latlong_coords";

        public override void OnRun(List<string> args)
        {
            Vector3 position = FloatingOriginManager.instance.GetGlobeCoords(Utils.PlayerTransform);
            ModConsole.Log($"Your current world position");
            ModConsole.Log($"Longitude: {position.x}");
            ModConsole.Log($"Latitude: {position.z}");
        }
    }
}
