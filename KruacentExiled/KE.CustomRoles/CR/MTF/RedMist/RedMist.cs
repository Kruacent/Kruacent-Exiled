using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp1509;
using KE.CustomRoles.API.Features;
using KE.CustomRoles.API.Interfaces;
using PlayerRoles;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KE.CustomRoles.CR.MTF.RedMist
{
    public class RedMistRole : KECustomRole, IColor
    {
        public override string InternalName => "RedMist";
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new()
            {
                ["en"] = new()
                {
                    [TranslationKeyName] = "The Red Mist",
                    [TranslationKeyDesc] = "todo",
                },
                ["fr"] = new()
                {
                    [TranslationKeyName] = "The Red Mist",
                    [TranslationKeyDesc] = "todo",
                },
                ["legacy"] = new()
                {
                    [TranslationKeyName] = "The Red Mist",
                    [TranslationKeyDesc] = "todo",
                }
            };
        }
        public override int MaxHealth { get; set; } = 200;
        public override RoleTypeId Role { get; set; } = RoleTypeId.ClassD;
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;
        public Color32 Color => new(255, 192, 203, 0);
        public override float SpawnChance { get; set; } = 0;

        //You're the legendary Fixer, the Red Mist. This role comes with multiple strengths such as increased speed (1.5 to 2 times the normal speed ) , more health (200 health), you spawn with a machete scp but better, The Mimicry also you can't use guns.
        //The Mimicry is a weapon that deals 100 damage per normal attacks and instead of inspecting you do a devastating vertical slash (30sec cd) doing 400 damage.
        //You have 2 abilities one is to manifest your E.G.O, an armor that boosts your abilities but drains your health over time if you don't do damage (you have life steal).
        //The second one is only useable when you manifested your E.G.O, and horizontal slash doing 600 damage and hitting everyone in the room (60sec cd)
        //If you let people on your team die you get weaker, you are a protector afterall.

        //faster, 200hp, machete mais 200 hp de dégats (75 pour les humains)
        //+ego : quick heal drain pause when attacking, 80 damage reduction faster
        //forward slash :  damage everything on its path max distance of a room

        public override HashSet<string> Abilities { get; } =
        [
            "ToggleEGO",
            "Spear",
            "OnRush",
            "GreaterSplitHorizontal",
        ];

        protected override void GiveInventory(Player player)
        {
            try
            {

                //ci
                Item i = player.AddItem(ItemType.SCP1509);
                
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
                

        }

        protected override void RoleAdded(Player player)
        {
            if (!player.ReferenceHub.TryGetComponent<EGO>(out _))
            {
                Log.Debug("adding comp");
                player.ReferenceHub.gameObject.AddComponent<EGO>();
            }
            base.RoleAdded(player);
        }

        protected override void RoleRemoved(Player player)
        {
            if (player.ReferenceHub.gameObject.TryGetComponent<EGO>(out var ego))
            {
                UnityEngine.Object.Destroy(ego);
            }
            base.RoleRemoved(player);
        }

        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Scp1509.Resurrecting += OnResurrecting;
            
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Scp1509.Resurrecting -= OnResurrecting;
            base.UnsubscribeEvents();
        }

        private void OnResurrecting(ResurrectingEventArgs ev)
        {
            Player player = ev.Player;
            if (!Check(player)) return;

            ev.IsAllowed = false;
        }

        

    }
}
