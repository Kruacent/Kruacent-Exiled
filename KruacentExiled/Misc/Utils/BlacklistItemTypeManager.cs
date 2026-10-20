using Exiled.API.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KruacentExiled.Misc.Utils
{
    public static class BlacklistItemTypeManager
    {
        public static IReadOnlyCollection<ItemType> BlackListItem = new List<ItemType>
        {
            ItemType.None,
            ItemType.MicroHID,
            ItemType.Jailbird,
            ItemType.GunSCP127,
            ItemType.ParticleDisruptor,
            ItemType.DebugRagdollMover,
            ItemType.MarshmallowItem,
            ItemType.SCP1507Tape,
            ItemType.Scp021J,
            ItemType.Snowball,
            ItemType.SpecialCoal
        };



        private static List<ItemType> authorized = null;


        public static IReadOnlyCollection<ItemType> Authorized
        {
            get
            {
                if(authorized == null)
                {
                    Fill();
                }
                return authorized;
            }
        }
        private static void Fill()
        {
            ItemType[] arr = Enum.GetValues(typeof(ItemType)).ToArray<ItemType>();
            authorized = new List<ItemType>();
            foreach(ItemType itemType in arr)
            {
                if (!BlackListItem.Contains(itemType))
                {
                    authorized.Add(itemType);
                }
            }


        }


        public static ItemType GetRandom()
        {
            return Authorized.GetRandomValue();
        }
    }
}
