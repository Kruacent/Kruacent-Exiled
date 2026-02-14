using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using InventorySystem.Items.Usables;
using KE.GlobalEventFramework.GEFE.API.Features;
using KE.GlobalEventFramework.GEFE.API.Interfaces;
using KE.Misc.Events.EventsArgs;
using KE.Utils.Extensions;
using MEC;
using PlayerRoles;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Player = Exiled.API.Features.Player;

namespace KE.GlobalEventFramework.Examples.GE
{
    public class SwapProtocol : GlobalEvent, IStart, IEvent
    {
        public override uint Id { get; set; } = 1051;
        public override string Name { get; set; } = "SwapProtocol";
        public override string Description { get; } = "EEEEEEEET c'est parti la roulette tourne";
        public override int WeightedChance { get; set; } = 0;

        private int numberSwap = 0;
        public int NumberOfSwap => numberSwap;

        /// <summary>
        /// Cooldown between each change in seconds
        /// </summary>
        public int Cooldown = 60;

        public IEnumerator<float> Start()
        {
            while (!Round.IsEnded)
            {
                yield return Timing.WaitForSeconds(Cooldown);
                numberSwap++;
                SwapAllPlayers();
            }
        }

        private void SwapAllPlayers()
        {
            List<Player> players = Player.List.ToList();
            Log.Debug($"[swapprotocol] player count : {players.Count}");

            if (players.Count < 2)
            {
                Log.Debug("[swapprotocol] not enough player");
                return;
            }

            players.ShuffleList();

            List<PlayerData> playersData = players.Select(p => new PlayerData(p)).ToList();

            for (int i = 0; i < players.Count; i++)
            {
                int sourceIndex = (i + 1) % players.Count;

                Player target = players[i];
                PlayerData dataToApply = playersData[sourceIndex];

                Log.Debug($"[swappotocol] swap {target.Nickname} from {sourceIndex}");

                ExecuteSwap(target, dataToApply);
            }
            
        }

        /// <summary>
        /// Move a player's data to another player
        /// </summary>
        /// <param name="target"></param>
        /// <param name="data"></param>
        private void ExecuteSwap(Player target, PlayerData data)
        {
            data.ApplyTo(target);
        }

        public void SubscribeEvent()
        {
            Misc.Features.SCPBuff.OnBuffingSCP += OnBuffingSCP;
        }

        public void UnsubscribeEvent()
        {
            Misc.Features.SCPBuff.OnBuffingSCP -= OnBuffingSCP;
        }

        private void OnBuffingSCP(BuffingSCPEventArgs ev)
        {
            if(NumberOfSwap > 0)
            {
                ev.IsAllowed = false;
            }
        }

        private sealed class PlayerData
        {
            public string Nickname { get; }
            public RoleTypeId Role { get; }
            public float MaxHealth { get; }
            public float Health { get; }
            public Vector3 PlayerScale { get; }
            public bool IsCuffed { get; }
            public List<ItemState> SavedItems { get; }
            public Dictionary<AmmoType, ushort> Ammo { get; }
            public List<EffectState> SavedEffects { get; }

            public PlayerData(Player player)
            {
                Nickname = player.Nickname;
                Role = player.Role.Type;
                MaxHealth = player.MaxHealth;
                Health = player.Health;
                PlayerScale = player.Scale;
                IsCuffed = player.IsCuffed;

                SavedItems = new List<ItemState>();
                foreach (Item item in player.Items)
                {
                    SavedItems.Add(new ItemState(item));
                }

                Ammo = player.Ammo.ToDictionary(k => k.Key.GetAmmoType(), v => v.Value);
                SavedEffects = player.ActiveEffects.Select(e => new EffectState(e)).ToList();
            }

            public void ApplyTo(Player target)
            {
                target.ClearInventory();

                target.Role.Set(Role, RoleSpawnFlags.None);

                
                if (target.PreviousRole == RoleTypeId.Spectator)
                {
                    Vector3 randomRoom;
                    if (!Warhead.IsDetonated)
                    {
                        randomRoom = Room.Random(ZoneType.HeavyContainment).GetValidPosition();
                    }
                    else
                    {
                        randomRoom = Room.List.First(r => r.Type == RoomType.Surface).GetValidPosition();
                    }
                    target.Position = randomRoom;
                }

                target.MaxHealth = MaxHealth;
                target.Health = Health;
                target.Scale = PlayerScale;
                if (IsCuffed && !target.IsCuffed) target.Handcuff(); else target.RemoveHandcuffs();

                foreach (ItemState state in SavedItems)
                {
                    Item newItem = target.AddItem(state.Type);
                    state.ApplyState(newItem);
                }

                foreach (var ammo in Ammo) target.SetAmmo(ammo.Key, ammo.Value);

                target.DisableAllEffects();
                foreach (EffectState state in SavedEffects) target.EnableEffect(state.Type, state.Intensity, state.Duration);
            }
        }

        private readonly struct ItemState
        {
            public ItemType Type { get; }
            public int AmmoInMag { get; }
            public float MicroEnergy { get; }
            public int JailbirdCharges { get; }

            public ItemState(Item item)
            {
                Type = item.Type;
                AmmoInMag = 0;
                MicroEnergy = 0;
                JailbirdCharges = 0;

                if (item is Firearm gun) AmmoInMag = gun.MagazineAmmo;
                else if (item is MicroHid micro) MicroEnergy = micro.Energy;
                else if (item is Jailbird jail) JailbirdCharges = jail.TotalCharges;
            }

            public void ApplyState(Item item)
            {
                if (item is Firearm gun) gun.MagazineAmmo = AmmoInMag;
                else if (item is MicroHid micro) micro.Energy = MicroEnergy;
                else if (item is Jailbird jail) jail.TotalCharges = JailbirdCharges;
            }
        }

        private readonly struct EffectState
        {
            public EffectType Type { get; }
            public byte Intensity { get; }
            public float Duration { get; }

            public EffectState(StatusEffectBase effect)
            {
                Type = effect.GetEffectType();
                Intensity = effect.Intensity;
                Duration = effect.Duration;
            }
        }
    }
}