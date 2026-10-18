using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Scp914;
using InventorySystem;
using InventorySystem.Items;
using KE.Misc.Features._914Upgrades;
using KE.Utils.API.Features;
using KruacentExiled.CustomItems.API.Features;
using KruacentExiled.CustomRoles.API.Features;
using KruacentExiled.CustomRoles.API.Interfaces;
using LabApi.Events.Arguments.Scp914Events;
using LabApi.Features.Interfaces;
using LabApi.Features.Wrappers;
using Scp914.Processors;
using System.Collections.Generic;
using System.Linq;
using static Scp914.Processors.FirearmItemProcessor;

namespace KruacentExiled.Misc.Features
{
    internal class _914 : LoadingMiscFeature<Base914Upgrade>
    {

        public override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Scp914.UpgradingPlayer += InternalUpgradingPlayer;
            LabApi.Events.Handlers.Scp914Events.ProcessingPickup += InternalUpgradingPickup;
            LabApi.Events.Handlers.Scp914Events.ProcessingInventoryItem += InternalUpgradingInventory;
            base.SubscribeEvents();
        }

        public override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Scp914.UpgradingPlayer -= InternalUpgradingPlayer;
            LabApi.Events.Handlers.Scp914Events.ProcessingPickup -= InternalUpgradingPickup;
            LabApi.Events.Handlers.Scp914Events.ProcessingInventoryItem -= InternalUpgradingInventory;
            base.UnsubscribeEvents();
        }



        internal void InternalUpgradingPlayer(UpgradingPlayerEventArgs ev)
        {
            if (!ev.IsAllowed)
            {
                return;
            }


            foreach(Base914Upgrade feature in _allLoadedFeatures)
            {
                bool flag = feature.InternalUpgradingPlayer(ev);
                if (flag)
                {
                    break;
                }
            }


        }

        internal void InternalUpgradingPickup(Scp914ProcessingPickupEventArgs ev)
        {
            bool isLucky = KECustomRole.Get(ev.Pickup.LastOwner).OfType<ILucky>().Any(r => r.LuckProfile == LuckProfile.UltimateLucky);
            if (!isLucky) return;
             
            ItemType type = ev.Pickup.Type;
            if (KECustomItem.TryGet(Exiled.API.Features.Pickups.Pickup.Get(ev.Pickup.Base), out _))
            {
                return;
            }

            if (!InventoryItemLoader.TryGetItem<ItemBase>(type, out var result))
            {
                return;
            }

            ItemType[] possibleOutputs = null;
                
            if (result.TryGetComponent<StandardItemProcessor>(out var component))
            {
                KELog.Debug("StandardItemProcessor");

                switch (ev.KnobSetting)
                {
                    case Scp914.Scp914KnobSetting.Rough:
                        possibleOutputs = component._roughOutputs;
                        break;
                    case Scp914.Scp914KnobSetting.Coarse:
                        possibleOutputs = component._coarseOutputs;
                        break;
                    case Scp914.Scp914KnobSetting.OneToOne:
                        possibleOutputs = component._oneToOneOutputs;
                        break;
                    case Scp914.Scp914KnobSetting.Fine:
                        possibleOutputs = component._fineOutputs;
                        break;
                    case Scp914.Scp914KnobSetting.VeryFine:
                        possibleOutputs = component._veryFineOutputs;
                        break;
                }
            }
            else if (result.TryGetComponent<FirearmItemProcessor>(out var compon))
            {
                KELog.Debug("FireArmItmeProcesspr");
                FirearmOutput[] fireOutputs = null;

                switch (ev.KnobSetting)
                {
                    case Scp914.Scp914KnobSetting.Rough:
                        fireOutputs = compon._roughOutputs;
                        break;
                    case Scp914.Scp914KnobSetting.Coarse:
                        fireOutputs = compon._coarseOutputs;
                        break;
                    case Scp914.Scp914KnobSetting.OneToOne:
                        fireOutputs = compon._oneToOneOutputs;
                        break;
                    case Scp914.Scp914KnobSetting.Fine:
                        fireOutputs = compon._fineOutputs;
                        break;
                    case Scp914.Scp914KnobSetting.VeryFine:
                        fireOutputs = compon._veryFineOutputs;
                        break;
                }

                if (fireOutputs != null)
                {
                    List<ItemType> weaponOutputsList = new List<ItemType>();

                    foreach (FirearmOutput element in fireOutputs)
                    {
                        int occurrences = (int)(element.Chance * 100);

                        for (int i = 0; i < occurrences; i++)
                        {
                            foreach (ItemType item in element.TargetItems)
                            {
                                weaponOutputsList.Add(item);
                            }
                        }
                    }

                    possibleOutputs = weaponOutputsList.ToArray();
                }
            }
            else
            {
                possibleOutputs = GetHardcodedOutputs(type, ev.KnobSetting);
            }

            if (possibleOutputs != null && possibleOutputs.Length > 0)
            {
                ItemType theItem = SelectBestItem(possibleOutputs);

                if (theItem != ItemType.None)
                {
                    Log.Debug($"LuckyItemSelected is {theItem} by 914");
                    Pickup.Create(theItem, ev.NewPosition);
                    ev.IsAllowed = false;
                    ev.Pickup.Destroy();
                }
            }
        }

        internal void InternalUpgradingInventory(Scp914ProcessingInventoryItemEventArgs ev)
        {
            bool isLucky = KECustomRole.Get(ev.Player).OfType<ILucky>().Any(r => r.LuckProfile == LuckProfile.UltimateLucky);
            if (!isLucky) return;

            ItemType type = ev.Item.Type;
            if (KECustomItem.TryGet(Exiled.API.Features.Items.Item.Get(ev.Item.Base), out _))
            {
                return;
            }

            if (!InventoryItemLoader.TryGetItem<ItemBase>(type, out var result))
            {
                return;
            }

            ItemType[] possibleOutputs = null;

            if (result.TryGetComponent<StandardItemProcessor>(out var component))
            {
                KELog.Debug("StandardItemProcessor");

                switch (ev.KnobSetting)
                {
                    case Scp914.Scp914KnobSetting.Rough:
                        possibleOutputs = component._roughOutputs;
                        break;
                    case Scp914.Scp914KnobSetting.Coarse:
                        possibleOutputs = component._coarseOutputs;
                        break;
                    case Scp914.Scp914KnobSetting.OneToOne:
                        possibleOutputs = component._oneToOneOutputs;
                        break;
                    case Scp914.Scp914KnobSetting.Fine:
                        possibleOutputs = component._fineOutputs;
                        break;
                    case Scp914.Scp914KnobSetting.VeryFine:
                        possibleOutputs = component._veryFineOutputs;
                        break;
                }
            }
            else if (result.TryGetComponent<FirearmItemProcessor>(out var compon))
            {
                KELog.Debug("FireArmItmeProcesspr");
                FirearmOutput[] fireOutputs = null;

                switch (ev.KnobSetting)
                {
                    case Scp914.Scp914KnobSetting.Rough:
                        fireOutputs = compon._roughOutputs;
                        break;
                    case Scp914.Scp914KnobSetting.Coarse:
                        fireOutputs = compon._coarseOutputs;
                        break;
                    case Scp914.Scp914KnobSetting.OneToOne:
                        fireOutputs = compon._oneToOneOutputs;
                        break;
                    case Scp914.Scp914KnobSetting.Fine:
                        fireOutputs = compon._fineOutputs;
                        break;
                    case Scp914.Scp914KnobSetting.VeryFine:
                        fireOutputs = compon._veryFineOutputs;
                        break;
                }

                if (fireOutputs != null)
                {
                    List<ItemType> weaponOutputsList = new List<ItemType>();

                    foreach (FirearmOutput element in fireOutputs)
                    {
                        int occurrences = (int)(element.Chance * 100);

                        for (int i = 0; i < occurrences; i++)
                        {
                            foreach (ItemType item in element.TargetItems)
                            {
                                weaponOutputsList.Add(item);
                            }
                        }
                    }

                    possibleOutputs = weaponOutputsList.ToArray();
                }
            }
            else
            {
                possibleOutputs = GetHardcodedOutputs(type, ev.KnobSetting);
            }

            if (possibleOutputs != null && possibleOutputs.Length > 0)
            {
                ItemType theItem = SelectBestItem(possibleOutputs);

                if (theItem != ItemType.None)
                {
                    Log.Debug($"LuckyItemSelected is {theItem} by 914");
                    ev.Player.RemoveItem(ev.Item);
                    ev.Player.AddItem(theItem);
                    ev.IsAllowed = false;
                }
            }
        }

        private ItemType[] GetHardcodedOutputs(ItemType inputItem, Scp914.Scp914KnobSetting knob)
        {
            KELog.Debug("HARCODING Item processor custom de je sais pas quoi là");

            if (inputItem == ItemType.MicroHID)
            {
                switch (knob)
                {
                    case Scp914.Scp914KnobSetting.Rough: return new[] { ItemType.None };
                    case Scp914.Scp914KnobSetting.Coarse: return new[] { ItemType.MicroHID };
                    case Scp914.Scp914KnobSetting.Fine: return new[] { ItemType.MicroHID };
                    case Scp914.Scp914KnobSetting.VeryFine: return new[] { ItemType.MicroHID };
                    default: return null;
                }
            }
            else if (inputItem == ItemType.ParticleDisruptor)
            {
                switch (knob)
                {
                    case Scp914.Scp914KnobSetting.Coarse: return new[] { ItemType.None };
                    case Scp914.Scp914KnobSetting.OneToOne: return new[] { ItemType.MicroHID };
                    default: return null;
                }
            }
            else if (inputItem == ItemType.Flashlight)
            {
                switch (knob)
                {
                    case Scp914.Scp914KnobSetting.Rough: return new[] { ItemType.None };
                    case Scp914.Scp914KnobSetting.OneToOne: return new[] { ItemType.GrenadeFlash };
                    case Scp914.Scp914KnobSetting.VeryFine: return new[] { ItemType.SCP207 };
                    default: return null;
                }
            }
            else if (inputItem == ItemType.SCP268)
            {
                switch (knob)
                {
                    case Scp914.Scp914KnobSetting.OneToOne: return new[] { ItemType.SCP207 };
                    case Scp914.Scp914KnobSetting.VeryFine: return new[] { ItemType.SCP500 };
                    default: return null;
                }
            }

            return null;
        }

        private ItemType SelectBestItem(ItemType[] items)
        {
            if (items == null || items.Length == 0)
            {
                return ItemType.None;
            }

            var groupes = items.GroupBy(item => item).OrderBy(groupe => groupe.Count());
            var triRarete = groupes.OrderBy(groupe => groupe.Count());
            var triPriority = triRarete.ThenByDescending(groupe => GetExceptionPriority(groupe.Key));

            int position = 1;
            foreach (var groupe in triRarete)
            {
                ItemType type = groupe.Key;
                int quanti = groupe.Count();
                int score = GetExceptionPriority(type);

                KELog.Debug($"Item N°{position}, Type : {type}, quantite : {quanti}, score : {score}");
                position++;
            }

            ItemType itemChoisi = triPriority.First().Key;
            KELog.Debug($"Item choisi : {itemChoisi}");

            return itemChoisi;
        }

        private int GetExceptionPriority(ItemType item)
        {
            if (item == ItemType.None) return 0;
            if (item.IsScp()) return 90;
            
            switch (item)
            {
                case ItemType.Jailbird: return 99;
                case ItemType.ParticleDisruptor: return 98;
                case ItemType.GunFRMG0: return 70;
                case ItemType.GunLogicer: return 65;
                case ItemType.GunShotgun: return 64;
                case ItemType.GunCrossvec: return 60;
                case ItemType.GunCOM15: return 59;
                case ItemType.ArmorHeavy: return 58;
                case ItemType.KeycardChaosInsurgency: return 57;
                case ItemType.ArmorCombat: return 56;
                case ItemType.ArmorLight: return 55;
                case ItemType.Adrenaline: return 54;
                case ItemType.GrenadeHE: return 52;
                case ItemType.Flashlight: return 51;
                case ItemType.GrenadeFlash: return 50;
                case ItemType.Medkit: return 45;
                case ItemType.Painkillers: return 40;
                case ItemType.GunA7: return 1;

                default: return 0;
            }
        }
    }
}
