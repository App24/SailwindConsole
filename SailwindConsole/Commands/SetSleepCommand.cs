using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SailwindConsole.Commands
{
    internal class SetSleepCommand : Command
    {
        public override string Name => "set_sleep";
        public override int MinArgs => 1;

        public override void OnRun(List<string> args)
        {
            float.TryParse(args[0], out float amount);
            if (amount > 0)
            {
                PlayerNeeds.sleep = amount;
                UISoundPlayer.instance.PlayOpenSound();
                ModConsole.Log($"Set player sleep to {amount}");
            }
        }
    }
}
