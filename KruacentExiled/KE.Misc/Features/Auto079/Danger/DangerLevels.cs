using Exiled.API.Features.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Misc.Features.Auto079.Danger
{
    public class DangerLevels
    {

        public int Danger { get; }

        public IReadOnlyCollection<Item> Items { get; }


        private DangerLevels(int danger, List<Item> items) 
        {
            Danger = danger;
            Items = items;
        }



        public static DangerLevels GetDanger(List<Item> items)
        {
            int danger = 0;

            foreach(Item item in items)
            {
                danger += GetDangerItem(item);
            }

            return new(danger, items);

        }

        public static int GetDangerItem(Item item)
        {
            int danger = 0;


            if (item.IsFirearm)
            {
                danger += 2;
            }

            if(item is MicroHid)
            {
                danger += 5;
            }

            if(item is Jailbird)
            {
                danger += 3;
            }



            switch (item.Type)
            {
                case ItemType.SCP207:
                case ItemType.AntiSCP207:
                    danger += 2;
                    break;
                case ItemType.ParticleDisruptor:
                    danger += 3;
                    break;
                case ItemType.SCP018:
                    danger += 4;
                    break;
            }



            return danger;


        }
    }
}
