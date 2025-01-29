
using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Toys;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using KE.Items.Interface;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KE.Items
{
    public class MainPlugin : Plugin<Config>
    {
        public override string Author => "Patrique & OmerGS";
        public override string Name => "KEItems";
        internal static MainPlugin Instance { get; private set; }
        public override Version Version => new Version(1, 0, 0);
        private Dictionary<Pickup, Light> pl = new Dictionary<Pickup, Light>();
        public override void OnEnabled()
        {
            Instance = this;
            CustomItem.RegisterItems();
            Exiled.Events.Handlers.Player.DroppedItem += Drop;
            Exiled.Events.Handlers.Player.PickingUpItem += Pick;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            CustomItem.UnregisterItems();
            Exiled.Events.Handlers.Player.DroppedItem -= Drop;
            Exiled.Events.Handlers.Player.PickingUpItem -= Pick;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;

            base.OnDisabled();
            Instance = null;
        }

        public void Pick(PickingUpItemEventArgs ev)
        {
            Pickup pickup = ev.Pickup;
            if (pl.ContainsKey(pickup))
            {
                Light val = pl[pickup];
                if (val != null)
                {
                    val.UnSpawn();
                    val.Destroy();
                }
                pl.Remove(pickup);
            }
        }
        public void Drop(DroppedItemEventArgs ev)
        {
            Pickup pickup = ev.Pickup;
            if (CustomItem.TryGet(pickup, out CustomItem item) && item is ILumosItem ci)
            {
                pl.Add(pickup, null);
            }

        }
        public void OnRoundStarted()
        {
            Timing.RunCoroutine(LightP());
        }

        internal IEnumerator<float> LightP()
        {
            
            foreach (var p in Pickup.List)
            {
                if (p != null)
                {
                    if (CustomItem.TryGet(p, out CustomItem ci) && ci is ILumosItem)
                        pl.Add(p, null);
                }

            }
            while (Round.InProgress)
            {
                Log.Debug("boop");

                foreach (var x in pl.ToList())
                {
                    if(CustomItem.TryGet(x.Key, out CustomItem cui) && cui is ILumosItem ci)
                    {
                        Light light = Light.Create(x.Key.Position, null, null, true, ci.Color);
                        light.Intensity = 0.5f;
                        Log.Debug("preif");
                        if (x.Value != null)
                        {
                            Log.Debug("pre val");
                            Light val = x.Value;
                            Log.Debug($"destroy light {val.Position}");
                            val.UnSpawn();
                            Log.Debug("pre destroy");
                            val.Destroy();
                        }
                        else
                            Log.Debug("first cretate");
                        Log.Debug("reasigne");
                        pl[x.Key] = light;
                        Log.Debug("post reasigne");
                        //Log.Debug(x.Key.Position+";"+x.Value.Position);
                    }
                    else
                    {
                        Light val = x.Value;
                        val.UnSpawn();
                        val.Destroy();
                        pl.Remove(x.Key);
                    }
                }
                Log.Debug("waiting");
                yield return Timing.WaitForSeconds(0.1f);
            }
            Log.Debug("end while");

        }

    }
}