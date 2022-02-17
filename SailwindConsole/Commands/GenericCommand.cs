using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SailwindConsole.Commands
{
    internal class GenericCommand : Command
    {
        private string name;
        private int minArgs;
        private Action<List<string>> onRun;
        public override string Name => name;
        override public int MinArgs => minArgs;

        public GenericCommand(string name, Action<List<string>> onRun, int minArgs)
        {
            this.name = name;
            this.onRun = onRun;
            this.minArgs = minArgs;
        }

        public GenericCommand(string name, Action<List<string>> onRun) : this(name, onRun, 0)
        {
        }

        public override void OnRun(List<string> args)
        {
            onRun?.Invoke(args);
        }
    }
}
