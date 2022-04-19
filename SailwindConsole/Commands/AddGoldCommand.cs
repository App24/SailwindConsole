using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SailwindConsole.Commands
{
    internal class AddGoldCommand : Command
    {
        public override string Name => "addGold";
        public override int MinArgs => 1;
        public override string Usage => "<amount>";

        public override string Description => "Increase amount of gold";

        public override void OnRun(List<string> args)
        {
            int.TryParse(args[0], out int amount);
            if (amount > 0)
            {
                PlayerGold.gold += amount;
                UISoundPlayer.instance.PlayGoldSound();
                ModConsoleLog.Log($"Added {amount} gold");
            }
            else
            {
                ModConsoleLog.Error("Cannot have a value below 0!");
            }
        }
    }
}
