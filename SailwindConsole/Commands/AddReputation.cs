using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SailwindConsole.Commands
{
    internal class AddReputation : Command
    {
        public override string Name => "add_reputation";
        public override int MinArgs => 2;

        public override void OnRun(List<string> args)
        {
            Enum.TryParse(args[0], out PortRegion region);
            int.TryParse(args[1], out int reputation);
            PlayerReputation.ChangeReputation(reputation, region);
            ModConsole.Log($"Added {reputation} reputation to {region}");
        }
    }
}
