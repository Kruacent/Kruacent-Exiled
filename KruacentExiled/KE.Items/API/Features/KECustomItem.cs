using Exiled.API.Features;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using KE.Items.API.Features.SpawnPoints;
using KE.Items.API.Interface;
using KE.Utils.API.Displays.DisplayMeow;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static KE.Items.API.Features.SpawnPoints.PoseRoomSpawnPointHandler;
using Pickup = Exiled.API.Features.Pickups.Pickup;

namespace KE.Items.API.Features
{
    public abstract class KECustomItem : CustomItem
    {

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


        public override uint Spawn(IEnumerable<SpawnPoint> spawnPoints, uint limit)
        {
            Log.Debug($"spawning {this.Name}");
            HashSet<SpawnPoint> spawns = spawnPoints.ToHashSet();
            uint num = 0;
            foreach (SpawnPoint spawnpoint in spawnPoints.Where(sp => sp is RoomSpawnPoint))
            {
                Pickup pickup;
                if (Exiled.Loader.Loader.Random.NextDouble() * 100.0 >= (double)spawnpoint.Chance || (limit != 0 && num >= limit))
                {
                    continue;
                }
                spawns.Remove(spawnpoint);
                RoomSpawnPoint room = spawnpoint as RoomSpawnPoint;
                ItemSpawn spawn = PoseRoomSpawnPointHandler.UseRandomPose(room.Room);
                Log.Debug(room.Room+ " : "+PoseRoomSpawnPointHandler.usablePose.Count(p => p.roomType == room.Room));
                Log.Debug($"spawning {this.Name} in {room.Room}" );

                if (spawn is not null)
                {
                    Log.Debug($"spawning custom pos");
                    pickup = Spawn(spawn.Position);
                }
                else
                {
                    Log.Error($"can't spawn in custom");
                    pickup = Spawn(spawnpoint.Position);
                }



                if (pickup is not null)
                {
                    num++;
                }

                    
            }

            return base.Spawn(spawns, limit-num);
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
