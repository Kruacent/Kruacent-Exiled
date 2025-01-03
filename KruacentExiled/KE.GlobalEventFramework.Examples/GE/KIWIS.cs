using Exiled.API.Features;
using MEC;
using PlayerRoles;
using Utils.NonAllocLINQ;
using System.Collections.Generic;
using System.Linq;
using KE.GlobalEventFramework.GEFE.API.Features;


namespace KE.GlobalEventFramework.Examples.GE
{
    /// <summary>
    /// The scp start with 2/3 of it's life and end with 4/3 of it's vanilla life 
    /// </summary>
    public class KIWIS : GlobalEvent
    {
        ///<inheritdoc/>
        public override uint Id { get; set; } = 1047;
        ///<inheritdoc/>
        public override string Name { get; set; } = "KIWIS";
        ///<inheritdoc/>
        public override string Description { get; set; } = "Kill It While It's Small";
        ///<inheritdoc/>
        public override int Weight { get; set; } = 1;
        ///<inheritdoc/>
        public override IEnumerator<float> Start()
        {
            var listScp = Player.List.ToList().Where(p => p.IsScp && p.Role.Type != RoleTypeId.Scp0492).ToList().ToDictionary(p => p, p => p.MaxHealth/3);

            //set the health of all starting scps to 2/3 of their vanilla max health
            listScp.ForEach(k =>
            {
                k.Key.MaxHealth = k.Value * 2;
                k.Key.Health = k.Key.MaxHealth;
            });

            yield return Timing.WaitUntilTrue(() => Round.ElapsedTime.TotalMinutes >= 15);

            listScp = listScp.Where(p => p.Key.IsScp && p.Key.Role.Type != RoleTypeId.Scp0492).ToList().ToDictionary(p => p.Key, p => p.Value);
            listScp.ForEach(k => {
                k.Key.MaxHealth += k.Value;
                k.Key.Heal(k.Value);
            });

            yield return Timing.WaitUntilTrue(() => Round.ElapsedTime.TotalMinutes >= 30);
            listScp = listScp.Where(p => p.Key.IsScp && p.Key.Role.Type != RoleTypeId.Scp0492).ToList().ToDictionary(p => p.Key, p => p.Value);
            listScp.ForEach(k => {
                k.Key.MaxHealth += k.Value;
                k.Key.Heal(k.Value);
            });

            yield return 0;
            
        }

    }
}
