using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using GlobalEventFramework.API.Features;
using GlobalEventFramework.API.Interfaces;
using KruacentExiled.CustomRoles.CustomSCPTeam;
using MEC;
using System.Collections.Generic;


namespace KruacentExiled.GlobalEvents.MiddleEvents
{
    /// <summary>
    /// Gives everyone a coin (except to SCPs)
    /// </summary>
    public class EveryoneGamble : MiddleEvent, IAsyncStart
    {
        public override string Name { get; set; } = "EGamble";
        public override string Description { get; set; } = "Everyone, throw the game now!";
        public override int WeightedChance { get; set; } = 1;

        public IEnumerator<float> Start()
        {
            yield return Timing.WaitForSeconds(1);


            foreach (var player in Player.Enumerable)
            {
                if (SCPTeam.IsSCP(player.ReferenceHub))
                {
                    continue;
                }
                else
                {
                    if (player.IsInventoryFull)
                    {
                        Pickup.CreateAndSpawn(ItemType.Coin, player.Position);
                    }
                    else
                    {
                        player.AddItem(ItemType.Coin);
                    }
                }
            }

        }
    }
}
