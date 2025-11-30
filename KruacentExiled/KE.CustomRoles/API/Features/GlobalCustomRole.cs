using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Pools;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using InventorySystem.Configs;
using KE.CustomRoles.API.Interfaces;
using KE.Utils.API.Displays.DisplayMeow;
using LiteNetLib4Mirror.Open.Nat;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.CustomRoles.API.Features
{

    public abstract class GlobalCustomRole : KECustomRole
    {
        public sealed override RoleTypeId Role { get; set; } = RoleTypeId.None;
        public abstract SideEnum Side { get; set; }
        public override bool KeepInventoryOnSpawn { get; set; } = true;
        public override bool RemovalKillsPlayer => false;

        public override string Name
        {
            get
            {
                return Side.ToString().ToUpper() + "_" + InternalName.RemoveSpaces();
            }
        }
        
        public override void AddRole(Player player)
        {
            SideEnum side = SideClass.Get(player.Role.Side);
            
            if (side != Side)
            {
                Log.Error($"tried to give a global custom role to a player in the wrong side ({side} instead of {Side})");
                return;
            }


            base.AddRole(player);
        }

        protected override void AttributeHealth(Player player)
        {
            player.MaxHealth *= MaxHealthMultiplicator;
            player.Health = player.MaxHealth;
        }




        public sealed override int MaxHealth { get; set; }
        public virtual float MaxHealthMultiplicator { get; set; } = 1;

        protected override void ShowMessage(Player player)
        {
            StringBuilder sb = StringBuilderPool.Pool.Get();
            sb.Append("<b>");
            IColor color = this as IColor;
            if (color != null)
            {
                sb.Append("<color=#");
                sb.Append(ColorUtility.ToHtmlStringRGB(color.Color));
                sb.Append(">");
            }


            sb.Append(PublicName);

            if (color != null)
            {
                sb.Append("</color>");
            }

            if (player.IsScp)
            {
                sb.Append(player.Role.Name);
            }


            sb.AppendLine("</b>");

            if (MainPlugin.SettingHandler.GetDescriptionsSettings(player))
            {
                sb.AppendLine(Description);
            }


            float delay = MainPlugin.SettingHandler.GetTime(player);

            DisplayHandler.Instance.AddHint(MainPlugin.CRHint, player, sb.ToString(), delay);
            StringBuilderPool.Pool.Return(sb);
        }



        public override bool IsAvailable(Player player)
        {
            if (CurrentNumberOfSpawn >= Limit) return false;
            return SideClass.Get(player.Role.Side) == Side;
        }
    }

    public enum SideEnum
    {
        Human,
        SCP,
        None,
    }

    public static class SideClass
    {
        public static SideEnum Get(Side side)
        {
            return side switch
            {
                Side.Scp => SideEnum.SCP,
                Side.Tutorial or Side.Mtf or Side.ChaosInsurgency => SideEnum.Human,
                Side.None => SideEnum.None,
                _ => SideEnum.None,
            };
        }
    }

}
