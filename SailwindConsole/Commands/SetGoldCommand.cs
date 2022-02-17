using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SailwindConsole.Commands
{
    internal class SetGoldCommand : Command
    {
        public override string Name => "set_gold";
        public override int MinArgs => 1;

        public override void OnRun(List<string> args)
        {
            int.TryParse(args[0], out int amount);
            PlayerGold.gold = amount;
            UISoundPlayer.instance.PlayGoldSound();
            ModConsole.Log($"Set player gold to {amount}");
        }
    }
}
