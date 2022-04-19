using SailwindModdingHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SailwindConsole.Commands
{
    internal class SpawnObjectCommand : Command
    {
        public override string Name => "spawnObject";
        public override int MinArgs => 1;
        public override string Usage => "<object name>";

        public override string Description => "Spawn an object at player's position";

        public override void OnRun(List<string> args)
        {
            string prefabName=string.Join(" ", args);
            GameObject prefab=Prefabs.GetPrefab(prefabName);
            if (!prefab)
            {
                ModConsoleLog.Error($"Cannot find a spawnable object by the name of {prefabName}");
                return;
            }

            GameObject gameObject = GameObject.Instantiate(prefab);
            gameObject.transform.position = Utils.PlayerTransform.position + (Vector3.up * 2);

            ShipItem shipItem = gameObject.GetComponent<ShipItem>();
            if (shipItem)
            {
                shipItem.sold = true;
            }

            ModConsoleLog.Log("Spawned object!");
        }
    }
}
