using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using KE.GlobalEventFramework.Examples.API.Feature.mf;
using KE.GlobalEventFramework.Examples.API.Interfaces;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.NonAllocLINQ;

namespace KE.GlobalEventFramework.Examples.API.MalfunctionEffects
{
    internal class Locks : MalfunctionEffect, IReversibleEffect
    {
        public override string Name { get; } = "Door locks terminated";
        public override string VoiceLine { get; } = "Malfunctions levels above . 50 percent . . terminating all door locks";
        public override string VoiceLineTranslated { get; } = "Malfunctions levels above 50%, terminating all door locks";
        public override sbyte MalfunctionActivation { get; } = 50;

        public string VoiceLineDeactivate { get; } = "Malfunctions back to more stable levels, reputting all door locks";
        public string VoiceLineDeactivateTranslated { get; } = "Malfunctions back to more stable levels, reputting all door locks";
        public sbyte MalfunctionDeactivation { get; } = 40;


        private Dictionary<Door, KeycardPermissions> doorkeys = new Dictionary<Door, KeycardPermissions>();
        public override void ActivateEffect()
        {
            Door.List.ToList().ForEach(d =>
            {
                if (d.IsKeycardDoor)
                {
                    doorkeys.Add(d, d.KeycardPermissions);
                    d.KeycardPermissions = KeycardPermissions.None;
                }
            });
        }

        public void DeactivateEffect()
        {
            doorkeys.ForEach(dk =>
            {
                dk.Key.KeycardPermissions = dk.Value;
            });
        }

    }
}
