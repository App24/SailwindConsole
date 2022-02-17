using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SailwindConsole.Commands
{
    internal class SetTimeCommand : Command
    {
        public override string Name => "set_time";
        public override int MinArgs => 1;

        public override void OnRun(List<string> args)
        {
            float.TryParse(args[0], out float time);
            if (time > 0)
            {
                Sun.sun.globalTime = time;
                UISoundPlayer.instance.PlayOpenSound();
                ModConsole.Log($"Time set to: {time}");
            }
        }
    }
}
