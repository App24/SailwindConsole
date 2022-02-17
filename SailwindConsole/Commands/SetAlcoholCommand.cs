using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SailwindConsole.Commands
{
    internal class SetAlcoholCommand : Command
    {
        public override string Name => "set_alcohol";
        public override int MinArgs => 1;

        public override void OnRun(List<string> args)
        {
            float.TryParse(args[0], out float amount);
            if (amount >= 0)
            {
                PlayerNeeds.alcohol = amount;
                Refs.playerMouthCol.PlayDrinkSound();
                ModConsole.Log($"Set player alcohol to {amount}");
            }
        }
    }
}
