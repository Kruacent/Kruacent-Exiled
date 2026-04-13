using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using KE.GlobalEventFramework.GEFE.API.Enums;
using KE.GlobalEventFramework.GEFE.API.Features;
using KE.GlobalEventFramework.GEFE.API.Features.Hints;
using KE.GlobalEventFramework.GEFE.API.Interfaces;
using KE.Utils.Extensions;
using MapGeneration;
using MEC;
using PlayerRoles.FirstPersonControl;
using UnityEngine;


namespace KE.GlobalEventFramework.Examples.GE
{
    public class RollBack: GlobalEvent, IAsyncStart
    {
        ///<inheritdoc/>
        public override uint Id { get; set; } = 10800;
        ///<inheritdoc/>
        public override string Name { get; set; } = "RollBack";
        ///<inheritdoc/>
        public override string Description { get; } = "Shit the server is lagging";
        public override string[] AltDescription => 
        [
            "Pov Omer"
        ];
        ///<inheritdoc/>
        public override int WeightedChance { get; set; } = 1;

        public static readonly float RefreshRate = 30;
        public static float Luck = 5;
        public override ImpactLevel ImpactLevel => ImpactLevel.High;

        private Dictionary<Player, (Vector3, Quaternion)> playerpos = new();

        ///<inheritdoc/>
        public IEnumerator<float> Start()
        {
            bool luck;
            while (base.IsActive)
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
