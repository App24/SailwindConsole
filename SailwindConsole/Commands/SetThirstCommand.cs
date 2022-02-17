using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SailwindConsole.Commands
{
    internal class SetThirstCommand : Command
    {
        public override string Name => "set_thirst";
        public override int MinArgs => 1;

        public override void OnRun(List<string> args)
        {
            float.TryParse(args[0], out float amount);
            if (amount > 0)
            {
                PlayerNeeds.water = amount;
                Refs.playerMouthCol.PlayDrinkSound();
                ModConsole.Log($"Set player thirst to {amount}");
            }
        }
    }
}
