using System.Collections.Generic;
using Exiled.API.Features;
using GlobalEventFrameworkEXILED.API.Utils;
using InventorySystem;
using System.Linq;
using Exiled.API.Features.Items;
using Exiled.API.Extensions;
using Exiled.API.Enums;
using InventorySystem.Items.Firearms.Attachments.Components;
using Exiled.API.Structs;
using MEC;
using GEFExiled.GEFE.API.Features;

namespace GEFExiled.GEFE.Examples.GE
{
    public class Shuffle : GlobalEvent
    {
        ///<inheritdoc/>
        public override int Id { get; set; } = 31;
        ///<inheritdoc/>
        public override string Name { get; set; } = "Shuffle";
        ///<inheritdoc/>
        public override string Description { get; set; } = "et ça fait roomba café dans le scp";
        ///<inheritdoc/>
        public override double Weight { get; set; } = 1;
        private Player[] players = Player.List.ToArray();
        private List<Item>[] inventories;
        ///<inheritdoc/>
        public override IEnumerator<float> Start()
        {
            Coroutine.LaunchCoroutine(Update());
            yield return 0;
        }

        public Shuffle()
        {
            inventories = new List<Item>[players.Length];
            for (int i = 0; i < players.Length; i++)
            {
                inventories[i] = players[i].Items.ToList();
            }
        }
        private IEnumerator<float> Update()
        {
            while (!Round.IsEnded)
            {
                yield return Timing.WaitForSeconds(UnityEngine.Random.Range(120, 240));


            }
        }
        //sprainte 9 fèr lé non ki rest
        private void Shuffling(int decale)
        {
            for (int i = 0; i < players.Length; i++)
            {

            }
        }

        private void ChangeInventory(int pid)
        {
            var fire = new Dictionary<Firearm, IEnumerable<AttachmentIdentifier>>();
            players[pid].ClearItems();
            foreach (Item item in inventories[pid])
            {
                //is a firearm
                if (item is Firearm firearm)
                {
                    fire.Add(firearm, firearm.AttachmentIdentifiers);
                    players[pid].AddItem(fire);
                }
                else
                {
                    players[pid].AddItem(item);
                }
            }
        }
    }
}
