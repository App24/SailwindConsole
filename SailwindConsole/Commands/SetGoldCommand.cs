using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SailwindConsole.Commands
{
    internal class SetGoldCommand : Command
    {
        public override string Name => "setGold";
        public override int MinArgs => 1;
        public override string Usage => "<amount>";

        public override string Description => "Set amount of gold you have";

        public override void OnRun(List<string> args)
        {
            int.TryParse(args[0], out int amount);
            if (amount >= 0)
            {
                PlayerGold.gold = amount;
                UISoundPlayer.instance.PlayGoldSound();
                ModConsoleLog.Log($"Set player gold to {amount}");
            }
            else
            {
                ModConsoleLog.Error("Cannot have a value below 0!");
            }
        }
    }
}
