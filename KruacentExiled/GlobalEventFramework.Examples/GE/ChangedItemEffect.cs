using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp330;
using InventorySystem.Items.Usables.Scp330;
using KruacentExiled.GlobalEventFramework.GEFE.API.Enums;
using KruacentExiled.GlobalEventFramework.GEFE.API.Features;
using KruacentExiled.GlobalEventFramework.GEFE.API.Interfaces;
using System;
using System.Collections.Generic;

namespace KruacentExiled.GlobalEventFramework.Examples.GE
{
    public class ChangedItemEffect : GlobalEvent,IStart ,IEvent
    {

        public override uint Id { get; set; } = 1080;
        ///<inheritdoc/>
        public override string Name { get; set; } = "SwitchItemEffect";
        ///<inheritdoc/>
        public override string Description { get; } = "Les effets des items ont changé";
        public override string[] AltDescription => new string[]
        {
            "Roulette russe"
        };
        ///<inheritdoc/>
        public override int WeightedChance { get; set; } = 0;

        public Dictionary<ItemType, ItemType> newEffects;
        public Dictionary<CandyKindID, CandyKindID> newCandyEffects;



        public override ImpactLevel ImpactLevel => ImpactLevel.High;

        public void Start()
        {
            


            ChangeItemsEffect();
            ChangeCandyEffect();
        }

        private void ChangeItemsEffect()
        {

            List<ItemType> usableList = new List<ItemType>();

            foreach (var item in (ItemType[])Enum.GetValues(typeof(ItemType)))
            {
                if (IsUsable(item) && item != ItemType.SCP330)
                {
                    Log.Debug("adding : " + item);
                    usableList.Add(item);
                }


            }
            newEffects = new Dictionary<ItemType, ItemType>();

            int switchNumber = UnityEngine.Random.Range(1, usableList.Count);

            for (int i = 0; i < usableList.Count; i++)
            {
                newEffects.Add(usableList[i], usableList[(i + switchNumber) % usableList.Count]);
            }

        }

        private void ChangeCandyEffect()
        {
            List<CandyKindID> candys = new List<CandyKindID>();
            foreach (var item in (CandyKindID[])Enum.GetValues(typeof(CandyKindID)))
            {
                if (item != CandyKindID.None)
                {
                    Log.Debug("adding : " + item);
                    candys.Add(item);
                }
            }

            newCandyEffects = new Dictionary<CandyKindID, CandyKindID>();

            int switchNumber = UnityEngine.Random.Range(1, candys.Count);

            for (int i = 0; i < candys.Count; i++)
            {

                CandyKindID old = candys[i];
                CandyKindID newCandy = candys[(i + switchNumber) % candys.Count];

                Log.Debug($"old = {old}  ; new = {newCandy}");

                newCandyEffects.Add(old, newCandy);
            }
        }





        public void SubscribeEvent()
        {
            Exiled.Events.Handlers.Player.UsingItemCompleted += OnUsingItemCompleted;
            Exiled.Events.Handlers.Scp330.EatingScp330 += OnEatingScp330;
            
        }
        public void UnsubscribeEvent()
        {
            Exiled.Events.Handlers.Player.UsingItemCompleted -= OnUsingItemCompleted;
            Exiled.Events.Handlers.Scp330.EatingScp330 -= OnEatingScp330;
        }

        public static bool IsUsable(ItemType type)
        {
            try
            {
                if (type.IsMedical() || type.IsThrowable() || type.IsScp()) return true;
                return false;
            }
            catch (Exception)
            {
                return false;
            }

        }

        private void OnEatingScp330(EatingScp330EventArgs ev)
        {
            if (!newCandyEffects.TryGetValue(ev.Candy.Kind, out CandyKindID newCandy))
            {
                Log.Debug("not found");
                return;
            }


            ev.IsAllowed = false;

            ev.Scp330.RemoveCandy(ev.Candy.Kind);
            Log.Debug($"old candy = {ev.Candy.Kind}");
            Log.Debug($"new candy = {newCandy}");



            Scp330.AvailableCandies[newCandy].ServerApplyEffects(ev.Player.ReferenceHub);
        }


        private void OnUsingItemCompleted(UsingItemCompletedEventArgs ev)
        {
            if (CustomItem.TryGet(ev.Item, out var _)) return; 

            if (!newEffects.ContainsKey(ev.Item.Type)) return;

            if(!newEffects.TryGetValue(ev.Usable.Type,out ItemType newUsable))
            {
                Log.Debug("not found");
                return;
            }


            Log.Debug($"item used : {ev.Usable.Type}, item effect : {newUsable}");
            ev.IsAllowed = false;
            Usable use = Item.Create(newUsable, ev.Player) as Usable;
            if (use is null)
            {
                Log.Error("Usable null stopping");
                return;
            }

            ev.Item.Destroy();
            use.Use(ev.Player);
        }
    }
}
