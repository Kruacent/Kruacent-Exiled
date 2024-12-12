using Exiled.API.Features;
using GEFExiled.GEFE.API.Features;
using MEC;
using PlayerRoles;
using Utils.NonAllocLINQ;
using System.Collections.Generic;
using System.Linq;


namespace GEFExiled.GEFE.Examples.GE
{
    public class KIWIS : GlobalEvent
    {
        public override int Id { get; set; } = 32;
        public override string Name { get; set; } = "KIWIS";
        public override string Description { get; set; } = "Kill It While It's Small";
        public override double Weight { get; set; } = 1;
        public override IEnumerator<float> Start()
        {
            var listScp = Player.List.ToList().Where(p => p.IsScp && p.Role.Type != RoleTypeId.Scp0492).ToList().ToDictionary(p => p, p => p.MaxHealth/3);

            yield return Timing.WaitUntilTrue(() => Round.ElapsedTime.TotalMinutes >= 15);
            listScp.ForEach(k => {
                k.Key.MaxHealth += k.Value;
                k.Key.Heal(k.Value);
            });

            yield return Timing.WaitUntilTrue(() => Round.ElapsedTime.TotalMinutes >= 30);
            listScp.ForEach(k => {
                k.Key.MaxHealth += k.Value;
                k.Key.Heal(k.Value);
            });

            yield return 0;
            
        }

    }
}
