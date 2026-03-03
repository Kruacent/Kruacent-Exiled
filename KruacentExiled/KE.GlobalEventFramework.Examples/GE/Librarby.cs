using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Player;
using KE.GlobalEventFramework.GEFE.API.Features;
using KE.GlobalEventFramework.GEFE.API.Interfaces;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using UnityEngine;
using VoiceChat.Codec;

namespace KE.GlobalEventFramework.Examples.GE
{
    /// <summary>
    /// You cannot talk to loud
    /// </summary>
    public class Librarby : GlobalEvent, IEvent, IChanceRedactable
    {
        ///<inheritdoc/>
        public override uint Id { get; set; } = 1091;
        ///<inheritdoc/>
        public override string Name { get; set; } = "Librarby";
        ///<inheritdoc/>
        public override string Description { get; } = "Ne parlez pas trop fort sinon vous subirez les conséquences !";
        ///<inheritdoc/>
        public override int WeightedChance => 1;

        public float ChanceRedacted => 0;

        private float MaxVolume = 0.7f;

        private OpusDecoder _decoder = new OpusDecoder();
        private float[] _pcmBuffer = new float[1920];

        public void SubscribeEvent()
        {
            Exiled.Events.Handlers.Player.VoiceChatting += PlayerYapping;
        }

        public void UnsubscribeEvent()
        {
            Exiled.Events.Handlers.Player.VoiceChatting -= PlayerYapping;
        }

        private void PlayerYapping(VoiceChattingEventArgs ev)
        {
            if (ev.Player.Role.Side == Side.Scp || ev.Player.Role == RoleTypeId.Tutorial) return;

            int decodedLength = _decoder.Decode(ev.VoiceMessage.Data, ev.VoiceMessage.DataLength, _pcmBuffer);
            float maxVolume = 0f;

            for (int i = 0; i < decodedLength; i++)
            {
                float absValue = Mathf.Abs(_pcmBuffer[i]);

                if (absValue > maxVolume)
                {
                    maxVolume = absValue;
                }
            }

            if (maxVolume > MaxVolume)
            {
                ev.Player.Explode();
            }
        }
    }
}