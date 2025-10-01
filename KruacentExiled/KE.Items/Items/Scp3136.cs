using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Items;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using KE.Items.API.Features;
using KE.Items.API.Interface;
using KE.Items.Items.PickupModels;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Items.Items
{
    [CustomItem(ItemType.SCP1576)]
    public class Scp3136 : KECustomItem, ICustomPickupModel
    {
        public override uint Id { get; set; } = 1057;
        public override string Name { get; set; } = "SCP-3136";
        public override string Description { get; set; } = "A map of the facility, you could draw your friends next to you";
        public override float Weight { get; set; } = 0.65f;

        private Dictionary<Faction,Vector3> _respawnPositions = new Dictionary<Faction,Vector3>
        {
            { Faction.FoundationStaff , Vector3.zero},
            { Faction.FoundationEnemy , Vector3.zero}
        };

        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 1,
            LockerSpawnPoints = new List<LockerSpawnPoint>()
            {
                new LockerSpawnPoint()
                {
                    Type = Exiled.API.Enums.LockerType.Scp1576Pedestal,
                    UseChamber = true,
                    Chance = .5f
                }
            }
        };

        public PickupModel PickupModel { get; }

        public Scp3136()
        {
            //PickupModel = new Scp3136PModel(this);
        }

        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.UsedItem += OnDrawing;
            Exiled.Events.Handlers.Server.RespawnedTeam += OnRespawnedTeam;
            PickupModel?.SubscribeEvents();
            base.SubscribeEvents();
        }
        protected override void UnsubscribeEvents()
        {
            PickupModel?.UnsubscribeEvents();
            Exiled.Events.Handlers.Player.UsedItem -= OnDrawing;
            Exiled.Events.Handlers.Server.RespawnedTeam -= OnRespawnedTeam;
            base.UnsubscribeEvents();
        }

        private void OnDrawing(UsedItemEventArgs ev)
        {
            if (!Check(ev.Item)) return;
            Scp1576 item = ((Scp1576)ev.Item);
            switch (ev.Player.Role.Side)
            {
                case Side.Mtf:
                    _respawnPositions[Faction.FoundationStaff] = ev.Player.Position; 
                    break;
                case Side.ChaosInsurgency:
                case Side.Tutorial:
                    _respawnPositions[Faction.FoundationEnemy] = ev.Player.Position;
                    break;
            }
            item.StopTransmitting();
            item.RemainingCooldown = 5*60;



        }

        private void OnRespawnedTeam(RespawnedTeamEventArgs ev)
        {
            Vector3 spawnPos = _respawnPositions[ev.Wave.TargetFaction];
            if (spawnPos == Vector3.zero) return;

            foreach (Player player in ev.Players)
            {
                player.Teleport(spawnPos);
                _respawnPositions[ev.Wave.TargetFaction] = Vector3.zero;
            }
        }



    }
}
