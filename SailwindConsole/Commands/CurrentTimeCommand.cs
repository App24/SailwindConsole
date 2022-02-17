using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SailwindConsole.Commands
{
    internal class CurrentTimeCommand : Command
    {
        public override string Name => "current_time";

        public override void OnRun(List<string> args)
        {
            ModConsole.Log($"Local Time: {Sun.sun.localTime:F2}");
            ModConsole.Log($"Global Time: {Sun.sun.globalTime:F2}");
            UISoundPlayer.instance.PlayOpenSound();
        }
    }
}
