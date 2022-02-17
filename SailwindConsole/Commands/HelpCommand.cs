using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SailwindConsole.Commands
{
    internal class HelpCommand : Command
    {
        public override string Name => "help";

        private List<Command> commands;

        public HelpCommand(List<Command> commands)
        {
            this.commands = commands;
        }

        public override void OnRun(List<string> args)
        {
            foreach (var command in commands)
            {
                ModConsole.Log($"<color=blue>{command.Name}</color>, required {command.MinArgs} arguments!");
            }
        }
    }
}
