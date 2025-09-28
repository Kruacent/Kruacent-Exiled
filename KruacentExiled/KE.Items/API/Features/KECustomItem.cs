using Exiled.API.Features;
using Exiled.CustomItems.API.Features;
using MHints = HintServiceMeow.Core.Models.Hints.Hint;
using System.Text;
using KE.Utils.API.Displays.DisplayMeow;
using MEC;
using HintServiceMeow.Core.Extension;
using HintServiceMeow.Core.Utilities;
using System.Reflection;
using KE.Items.API.Interface;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features.Pickups;
using UnityEngine;
using Exiled.API.Features.Spawn;

namespace KE.Items.API.Features
{
    public abstract class KECustomItem : CustomItem
    {

        public virtual IEnumerable<ReplaceItem> RoomsToReplaceItems { get; protected set; } = null;
        protected override void ShowPickedUpMessage(Player player)
        {
            Log.Debug("pickup");
            Message(this, player, true);
        }

        protected override void ShowSelectedMessage(Player player)
        {
            Log.Debug("select");
            Message(this, player);
        }


        public override void SpawnAll()
        {
            if(RoomsToReplaceItems != null)
            {
                foreach(ReplaceItem replaceItem in RoomsToReplaceItems)
                {
                    foreach(Room room in Room.List.Where(r => r.Type == replaceItem.Room))
                    {
                        uint limit = 0;
                        foreach(Pickup pickup in room.Pickups.Where(p => p.Type == replaceItem.ItemToReplace))
                        {
                            if (limit <= replaceItem.LimitPerRoom && Exiled.Loader.Loader.Random.NextDouble() * 100.0 >= (double)replaceItem.Chance)
                            {
                                ReplacePickup(pickup);
                                limit++;
                            }
                        }
                    }
                }
            }


            base.SpawnAll();
        }


        public void ReplacePickup(Pickup pickup)
        {
            Vector3 position = pickup.Position;
            pickup.Destroy();
            Spawn(position);
        }


        internal static void Message(CustomItem c, Player player, bool pickedUp = false)
        {

            StringBuilder builder = new();

            if (MainPlugin.Instance.SettingsHandler.GetPrefixes(player))
            {
                if (pickedUp)
                {
                    builder.Append("(P)");
                }
                else
                {
                    builder.Append("(I)");
                }
            }
            else
            {
                if (pickedUp)
                {
                    builder.Append("You've picked up ");
                }
                else
                {
                    builder.Append("You've selected ");
                }
            }

            builder.AppendLine($"<b>{c.Name}</b>");
            if (MainPlugin.Instance.SettingsHandler.GetDescriptionsSettings(player))
            {
                builder.AppendLine(c.Description);
                if (c is IUpgradableCustomItem ci)
                {
                    builder.Append("<b>");
                    foreach (var a in ci.Upgrade)
                    {
                        builder.Append(a.Key);
                        builder.Append(" (");
                        builder.Append(a.Value.Chance);
                        builder.Append("%) -> ???");
                    }
                    builder.AppendLine("</b>");
                }

            }



            float delay = MainPlugin.Instance.SettingsHandler.GetTime(player);
            DisplayHandler.Instance.AddHint(MainPlugin.HintPlacement, player, builder.ToString(), delay);


        }




        public static void ItemEffectHint(Player player, string text)
        {
            float delay = MainPlugin.Instance.SettingsHandler.GetTimeEffect(player);


            DisplayHandler.Instance.AddHint(MainPlugin.ItemEffectPlacement, player, text, delay);
        }


    }
}
