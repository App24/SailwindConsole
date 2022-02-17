using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SailwindConsole.Commands
{
    internal class AddGoldCommand : Command
    {
        public override string Name => "add_gold";
        public override int MinArgs => 1;

        public override void OnRun(List<string> args)
        {
            int.TryParse(args[0], out int amount);
            PlayerGold.gold += amount;
            UISoundPlayer.instance.PlayGoldSound();
            ModConsole.Log($"Added {amount} gold");
        }
    }
}
