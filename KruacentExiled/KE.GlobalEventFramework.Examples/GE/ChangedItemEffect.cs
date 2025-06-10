using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Player;
using KE.GlobalEventFramework.GEFE.API.Features;
using KE.GlobalEventFramework.GEFE.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KE.GlobalEventFramework.Examples.GE
{
    public class ChangedItemEffect : GlobalEvent,IStart ,IEvent
    {

        public override uint Id { get; set; } = 1080;
        ///<inheritdoc/>
        public override string Name { get; set; } = "SwitchItemEffect";
        ///<inheritdoc/>
        public override string Description { get; set; } = "Les effets des items ont changé";
        ///<inheritdoc/>
        public override int Weight { get; set; } = 1;

        public Dictionary<ItemType, ItemType> newEffects;
        private List<ItemType> _usableList = new();

        private int _switchNumber =0;
        public int SwitchNumber { get { return _switchNumber; } }

        public IEnumerator<float> Start()
        {
            Log.Info("before foreach");
            foreach (var item in (ItemType[])Enum.GetValues(typeof(ItemType)))
            {
                if (IsUsable(item))
                {
                    Log.Info("adding : " + item);
                    _usableList.Add(item);
                }
            }
            Log.Info("after foreach");
            _usableList = _usableList.Distinct().ToList();


            _switchNumber = UnityEngine.Random.Range(1,_usableList.Count);
            newEffects = new Dictionary<ItemType, ItemType>();
            
            for(int i =0;i < _usableList.Count; i++)
            {
                newEffects.Add(_usableList[i], _usableList[(i+SwitchNumber)%_usableList.Count]);
            }

            yield return 0;
        }
        public void SubscribeEvent()
        {
            Exiled.Events.Handlers.Player.UsingItemCompleted += OnUsingItemCompleted;
            
        }
        public void UnsubscribeEvent()
        {
            Exiled.Events.Handlers.Player.UsingItemCompleted -= OnUsingItemCompleted;
        }

        public static bool IsUsable(ItemType type)
        {
            if (type.IsMedical() || type.IsThrowable() || type.IsScp()) return true;
            return false;
        }


        private void OnUsingItemCompleted(UsingItemCompletedEventArgs ev)
        {
            ev.IsAllowed = false;
            ItemType newUsable = newEffects[ev.Usable.Type];
            Log.Debug($"item used : {ev.Usable}, item effect : {newUsable}");
            Usable use = Usable.Create(newUsable, ev.Player) as Usable;
            if (use is null)
            {
                Log.Error("Usable null stopping");
            }
            use.Use(ev.Player);
        }
    }
}
