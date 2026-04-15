using Exiled.API.Features;
using KE.CustomRoles.API.Features.Abilities;
using KE.CustomRoles.API.Interfaces.Ability;
using KE.CustomRoles.CR.Scientist;
using KE.Utils.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.CustomRoles.Abilities.EmptyAbilities
{
    public class NumberCheckpointEmptyAbility : EmptyAbility, IDynamicName
    {
        public override string Name { get; } = "NumberCheckpoints";

        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Remaining Checkpoint%S% : %remain%/%total%",
                    [TranslationKeyDesc] = "",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Checkpoint%S% restant : %remain%/%total% ",
                    [TranslationKeyDesc] = "",
                }
            };
        }

        public void GetName(StringBuilder sb, Player player)
        {
            int remaining = ZoneManager.GetNumberCheckpoints(player);
            int total = ZoneManager.DoorToOpen.Count;
            KELog.Debug("remainig"+remaining);

            if(remaining > 0)
            {
                string n = GetTranslation(player, TranslationKeyName).Replace("%remain%", remaining.ToString()).Replace("%total%", total.ToString());
                if(remaining > 1)
                {
                    n = n.Replace("%S%", "s");
                }
                else
                {
                    n = n.Replace("%S%", string.Empty);
                }

                sb.Append(n);
            }
        }

    }
}
