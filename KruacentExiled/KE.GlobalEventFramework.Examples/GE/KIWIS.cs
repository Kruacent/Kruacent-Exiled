using Exiled.API.Features;
using MEC;
using PlayerRoles;
using Utils.NonAllocLINQ;
using System.Collections.Generic;
using System.Linq;
using KE.GlobalEventFramework.GEFE.API.Interfaces;
using KE.GlobalEventFramework.GEFE.API.Features;
using KE.GlobalEventFramework.GEFE.API.Features.Hints;
using KE.GlobalEventFramework.GEFE.API.Enums;

namespace KE.GlobalEventFramework.Examples.GE
{
    /// <summary>
    /// The scp start with 2/3 of it's life and end with 4/3 of it's vanilla life 
    /// </summary>
    public class KIWIS : GlobalEvent, IAsyncStart
    {
        ///<inheritdoc/>
        public override uint Id { get; set; } = 1047;
        ///<inheritdoc/>
        public override string Name { get; set; } = "KIWIS";
        ///<inheritdoc/>
        public override string Description { get; } = "Kill It While It's Small";
        public override ImpactLevel ImpactLevel => ImpactLevel.Medium;
        ///<inheritdoc/>
        public override int WeightedChance { get; set; } = 1;
        ///<inheritdoc/>
        public IEnumerator<float> Start()
        {
            var listScp = Player.List.ToList().Where(p => p.IsScp && p.Role.Type != RoleTypeId.Scp0492).ToList().ToDictionary(p => p, p => p.MaxHealth/3);


            yield return Timing.WaitUntilTrue(() => Round.ElapsedTime.TotalMinutes >= 15);

            listScp = listScp.Where(p => p.Key.IsScp && p.Key.Role.Type != RoleTypeId.Scp0492).ToList().ToDictionary(p => p.Key, p => p.Value);
            listScp.ForEach(k => {
                k.Key.MaxHealth += k.Value;
                k.Key.Heal(k.Value);
            });

            yield return Timing.WaitUntilTrue(() => Round.ElapsedTime.TotalMinutes >= 30 || Warhead.IsDetonated);
            listScp = listScp.Where(p => p.Key.IsScp && p.Key.Role.Type != RoleTypeId.Scp0492).ToList().ToDictionary(p => p.Key, p => p.Value);
            listScp.ForEach(k => {
                k.Key.MaxHealth += k.Value;
                k.Key.Heal(k.Value);
            });

            yield return 0;
            
        }

    }
}
