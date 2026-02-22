using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using KE.CustomRoles.API.Features;
using MEC;
using PlayerRoles;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KE.CustomRoles.CR.Guard
{
    internal class Introvert : KECustomRole
    {
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new()
            {
                ["en"] = new()
                {
                    [TranslationKeyName] = "Introvert",
                    [TranslationKeyDesc] = "your better by yourself",
                },
                ["fr"] = new()
                {
                    [TranslationKeyName] = "Introverti",
                    [TranslationKeyDesc] = "Tu n'aimes pas trop les humains",
                }
            };
        }
        public override int MaxHealth { get; set; } = 100;
        public override RoleTypeId Role { get; set; } = RoleTypeId.FacilityGuard;
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;

        public override float SpawnChance { get; set; } = 100;

        public override List<string> Inventory { get; set; } = new List<string>()
        {
          $"{ItemType.KeycardGuard}",
          $"{ItemType.GunFSP9}",
          $"{ItemType.Medkit}",
          $"{ItemType.GrenadeFlash}",
          $"{ItemType.GrenadeFlash}",
          $"{ItemType.ArmorLight}",
        };

        public override Dictionary<AmmoType, ushort> Ammo { get; set; } = new Dictionary<AmmoType, ushort>()
        {
          { AmmoType.Nato556, 60}
        };

        public override void Init()
        {
            _enabled = new();
            base.Init();
        }


        protected override void RoleAdded(Player player)
        {
            _enabled[player] = false;
            //Timing.RunCoroutine(BuffAlone(player));
            base.RoleAdded(player);
        }


        protected override void RoleRemoved(Player player)
        {
            SyncBuff(player, false);
            _enabled.Remove(player);
            base.RoleRemoved(player);
        }


        private const byte MovementBoostIntensity = 5;

        private Dictionary<Player, bool> _enabled;


        private IEnumerator<float> BuffAlone(Player player)
        {

            while (Check(player))
            {
                yield return Timing.WaitForSeconds(1f);
                SyncBuff(player, player.CurrentRoom.Players.Count(p => p != player) == 0);
            }
        }

        private void SyncBuff(Player player,bool alone)
        {
            MovementBoost boost = player.GetEffect<MovementBoost>();

            if (alone && !_enabled[player])
            {
                Log.Debug("add buff to "+player.Nickname);
                
                boost.Intensity += 5;
                _enabled[player] = true;
            }


            if (!alone && _enabled[player])
            {
                Log.Debug("remove buff to " + player.Nickname);
                boost.Intensity -= 5;
                _enabled[player] = false;
            }


        }


    }
}
