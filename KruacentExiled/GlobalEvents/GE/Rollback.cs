using System.Collections.Generic;
using Exiled.API.Features;
using KE.Utils.Extensions;
using GlobalEventFramework.API.Enums;
using GlobalEventFramework.API.Features;
using GlobalEventFramework.API.Interfaces;
using MEC;
using UnityEngine;


namespace KruacentExiled.GlobalEvents.GE
{
    public class RollBack: GlobalEvent, IAsyncStart
    {
        ///<inheritdoc/>
        public override string Name { get; set; } = "RollBack";
        ///<inheritdoc/>
        public override string Description { get; } = "Shit the server is lagging";
        public override string[] AltDescription => new string[]
        {
            "Pov Omer"
        };
        ///<inheritdoc/>
        public override int DefaultWeightedChance { get; } = 1;

        public static readonly float RefreshRate = 30;
        public static float Luck = 5;
        public override ImpactLevel ImpactLevel => ImpactLevel.High;

        private Dictionary<Player, (Vector3, Quaternion)> playerpos = new Dictionary<Player, (Vector3, Quaternion)>();

        ///<inheritdoc/>
        public IEnumerator<float> Start()
        {
            bool luck;
            while (IsActive)
            {
                yield return Timing.WaitForSeconds(RefreshRate);
                luck = Luck > Random.Range(0f,100f);
                foreach(Player p in Player.List)
                {

                    if (luck)
                    {
                        p.Teleport(playerpos[p].Item1);
                        p.Rotation = playerpos[p].Item2;
                    }

                    if (p.IsAlive && Lift.Get(p.Position) is null && p.Zone.IsSafe())
                    {
                        playerpos[p] = (p.Position, p.Rotation);
                    }

                    


                }

            }


        }



        
    }
}
