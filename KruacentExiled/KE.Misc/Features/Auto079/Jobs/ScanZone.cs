using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using Utils.NonAllocLINQ;
using Camera = Exiled.API.Features.Camera;

namespace KE.Misc.Features.Auto079.Jobs
{
    public class ScanZone : Job
    {

        private Dictionary<Player, List<Item>> InventoryGuess => npc.InventoryGuess;


        protected override IEnumerator<float> Started()
        {

            //List<ZoneType> zones = ZoneOrder();
            List<ZoneType> zones = new List<ZoneType>()
            {
                ZoneType.HeavyContainment
            };



            foreach (ZoneType zone in zones)
            {
                
                foreach (Camera camera in Camera.Get(c => c.Zone == zone))
                {
                    int cost = (int)Math.Ceiling((float)(npc.Role.GetSwitchCost(camera)/2));
                    Log.Debug($"trying to switch to cam {camera.Name} ({cost})");

                    yield return Timing.WaitUntilTrue(() => cost < npc.Role.Energy);
                    npc.Role.Energy -= cost;
                    npc.Role.Camera = camera;

                    


                    List<Player> scanned = ScanRoom(camera);

                    Log.Debug($"{npc.Role.Energy} / {npc.Role.MaxEnergy}");
                    yield return Timing.WaitForSeconds(WaitTime);
                    
                    foreach(Player p in scanned)
                    {
                        if (InventoryGuess.ContainsKey(p))
                        {
                            InventoryGuess.Add(p, new());
                        }
                        InventoryGuess[p].Add(p.CurrentItem);

                    }


                    PingPlayer(scanned.FirstOrDefault());


                }
                
            }


        }
        


        public void PingPlayer(Player playerToPing)
        {
            if (playerToPing is null) return;
            Log.Debug($"ping {playerToPing.NetId} at {playerToPing.Position}");
            npc.TryPing(playerToPing.Position, PingType.Human);
        }




        public List<Player> ScanRoom(Camera camera)
        {
            List<Player> playerFound = new();
            //todo add blind spots
            // eg 75% chance to be spotted when under a cam
            if (!camera.Room.Players.IsEmpty())
            {
                playerFound.AddRange(camera.Room.Players.Where(p => !p.IsScp));
            }
            return playerFound;
        }

        private List<ZoneType> ZoneOrder()
        {
            List<ZoneType> result = new();


            result.Add(npc.Scp.Zone);

            if (!Map.IsLczDecontaminated)
            {
                result.AddIfNotContains(ZoneType.LightContainment);
            }
            result.AddIfNotContains(ZoneType.HeavyContainment);
            result.AddIfNotContains(ZoneType.Entrance);
            result.AddIfNotContains(ZoneType.Surface);


            return result;

        }

    }
}
