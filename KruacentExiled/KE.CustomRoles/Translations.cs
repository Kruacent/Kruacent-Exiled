using Exiled.API.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.CustomRoles
{
    public class Translations : ITranslation
    {
        public string GettingNewRole { get; set; } = "<b>%Name%</b>\n %Desc%";


        [Description("Russe (1050)")]
        public string RusseDesc { get; set; } = "Tu es un<color=#FFC0CB>maitre de jeu</color> \nyou should play russian roulette with the others";
        public string RussePublicName { get; set; } = "Russian";


        [Description("DBoyInShape (1058)")]
        public string InShapeDesc { get; set; } = "Dammmmnnnnnnn les gates"; // aucune idée comment traduire ça
        public string InShapePublicName { get; set; } = "DBoyInShape";


        [Description("Enfant (1041)")]
        public string EnfantDesc { get; set; } = "You are a <color=#FFC0CB>Kid</color> \ndo not the kid \nyou start with a rainbow candy (in theory) \n you're a bit smaller";
        public string EnfantPublicName { get; set; } = "kid";


        [Description("Mime (1053)")]
        public string MimePublicName { get; set; } = "Mime";


        [Description("ChiefGuard (1046)")]
        public string ChiefGuardPublicName { get; set; } = "Chief Guard";
        public string ChiefGuardDesc { get; set; } = "Tu es un <color=#70C3FF>Chef des gardes du site</color> \nT'as une carte de private \net un crossvec";


    }
}
